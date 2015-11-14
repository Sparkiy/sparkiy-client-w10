//*********************************************************
// 
//  DirectXPanelBase.cpp
//  for Sparkiy 
// 
//*********************************************************

#pragma once

#include "pch.h"
#include "DirectXPanelBase.h"
#include "DirectXHelper.h"
#include <windows.ui.xaml.media.dxinterop.h>

using namespace sparkiy_Engine_Graphics_DirectX;
using namespace Platform;
using namespace Microsoft::WRL;
using namespace Windows::ApplicationModel;
using namespace Windows::Foundation;
using namespace Windows::Foundation::Collections;
using namespace Windows::Graphics::Display;
using namespace Windows::System::Threading;
using namespace Windows::UI;
using namespace Windows::UI::Core;
using namespace Windows::UI::Input::Inking;
using namespace Windows::Storage::Streams;
using namespace Windows::UI::Xaml;
using namespace Windows::UI::Xaml::Input;
using namespace Windows::UI::Xaml::Media;
using namespace Windows::UI::Xaml::Interop;
using namespace Concurrency;
using namespace DirectX;
using namespace D2D1;
using namespace DX;

// TODO Retrieve from device
static const float dipsPerInch = 96.0f;


/// <summary>
/// Initializes a new instance of the <see cref="DirectXPanelBase"/> class.
/// </summary>
DirectXPanelBase::DirectXPanelBase()
{	
	// Set default values
	this->isLoadingComplete = false;
	this->backgroundColor = D2D1::ColorF(D2D1::ColorF::White);
	this->alphaMode = DXGI_ALPHA_MODE_UNSPECIFIED; // TODO Change this to allow alpha later
	this->compositionScaleX = 1.0f;
	this->compositionScaleY = 1.0f;
	this->height = 1.0f;
	this->width = 1.0f;

	// Attach to events
	this->SizeChanged += ref new Windows::UI::Xaml::SizeChangedEventHandler(this, &DirectXPanelBase::OnSizeChanged);
	this->CompositionScaleChanged += ref new Windows::Foundation::TypedEventHandler<SwapChainPanel^, Object^>(this, &DirectXPanelBase::OnCompositionScaleChanged);
	Application::Current->Suspending += ref new SuspendingEventHandler(this, &DirectXPanelBase::OnSuspending);
	Application::Current->Resuming += ref new EventHandler<Object^>(this, &DirectXPanelBase::OnResuming);
}

/// <summary>
/// Creates the device independent resources.
/// </summary>
void DirectXPanelBase::CreateDeviceIndependentResources()
{
	D2D1_FACTORY_OPTIONS options;
	ZeroMemory(&options, sizeof(D2D1_FACTORY_OPTIONS));

#if defined(_DEBUG)
	// Enable D2D debugging via SDK Layers when in debug mode.
	options.debugLevel = D2D1_DEBUG_LEVEL_INFORMATION;
#endif

	ThrowIfFailed(
		D2D1CreateFactory(
			D2D1_FACTORY_TYPE_SINGLE_THREADED,
			__uuidof(ID2D1Factory2),
			&options,
			&this->d2dFactory));
}

/// <summary>
/// Creates the device resources.
/// </summary>
void DirectXPanelBase::CreateDeviceResources()
{
	// This flag adds support for surfaces with a different color channel ordering than the API default.
	// It is recommended usage, and is required for compatibility with Direct2D.
	UINT creationFlags = D3D11_CREATE_DEVICE_BGRA_SUPPORT;

#if defined(_DEBUG)
	// If the project is in a debug build, enable debugging via SDK Layers with this flag.
	//creationFlags |= D3D11_CREATE_DEVICE_DEBUG;
#endif

	// This array defines the set of DirectX hardware feature levels this application will support.
	// Note the ordering should be preserved.
	// Don't forget to declare your application's minimum required feature level in its
	// description.  All applications are assumed to support 9.1 unless otherwise stated.
	D3D_FEATURE_LEVEL featureLevels[] =
	{
		D3D_FEATURE_LEVEL_11_1,
		D3D_FEATURE_LEVEL_11_0,
		D3D_FEATURE_LEVEL_10_1,
		D3D_FEATURE_LEVEL_10_0,
		D3D_FEATURE_LEVEL_9_3,
		D3D_FEATURE_LEVEL_9_2,
		D3D_FEATURE_LEVEL_9_1
	};

	// Create the DX11 API device object, and get a corresponding context.
	ComPtr<ID3D11Device> device;
	ComPtr<ID3D11DeviceContext> context;
	ThrowIfFailed(
		D3D11CreateDevice(
			nullptr,                    // Specify null to use the default adapter.
			D3D_DRIVER_TYPE_HARDWARE,
			0,
			creationFlags,              // Optionally set debug and Direct2D compatibility flags.
			featureLevels,              // List of feature levels this application can support.
			ARRAYSIZE(featureLevels),
			D3D11_SDK_VERSION,          // Always set this to D3D11_SDK_VERSION for Windows Store applications.
			&device,                    // Returns the Direct3D device created.
			NULL,                       // Returns feature level of device created.
			&context));                 // Returns the device immediate context.
			

	// Get D3D11.1 device
	ThrowIfFailed(
		device.As(&this->d3dDevice));

	// Get D3D11.1 context
	ThrowIfFailed(
		context.As(&this->d3dContext));

	// Get underlying DXGI device of D3D device
	ComPtr<IDXGIDevice> dxgiDevice;
	ThrowIfFailed(
		this->d3dDevice.As(&dxgiDevice));

	// Get D2D device
	ThrowIfFailed(
		this->d2dFactory->CreateDevice(dxgiDevice.Get(), &this->d2dDevice));

	// Get D2D context
	ThrowIfFailed(
		this->d2dDevice->CreateDeviceContext(
			D2D1_DEVICE_CONTEXT_OPTIONS_NONE,
			&this->d2dContext));

	// Set D2D text anti-alias mode to gray-scale to ensure proper rendering of text on intermediate surfaces.
	this->d2dContext->SetTextAntialiasMode(D2D1_TEXT_ANTIALIAS_MODE_GRAYSCALE);
}

/// <summary>
/// Creates the size dependent resources.
/// </summary>
void DirectXPanelBase::CreateSizeDependentResources()
{
	// Ensure the rendering state has been released
	this->ReleaseDependentObjects();

	// Set render target size to the rendered size of the panel including the composition scale, 
	// defaulting to the minimum of 1px if no size was specified.
	this->renderTargetWidth = this->width * this->compositionScaleX;
	this->renderTargetHeight = this->height * this->compositionScaleY;

	// If the swap chain already exists, then resize it; Otherwise, create a new one.
	if (this->swapChain != nullptr)
		this->ResizeSwapChain();
	else this->CreateSwapChain();

	// Ensure the physical pixel size of the swap chain takes into account both the XAML SwapChainPanel's logical layout size and 
	// any cumulative composition scale applied due to zooming, render transforms, or the system's current scaling plateau.
	// For example, if a 100x100 SwapChainPanel has a cumulative 2x scale transform applied, we instead create a 200x200 swap chain 
	// to avoid artifacts from scaling it up by 2x, then apply an inverse 1/2x transform to the swap chain to cancel out the 2x transform.
	DXGI_MATRIX_3X2_F inverseScale = { 0 };
	inverseScale._11 = 1.0f / this->compositionScaleX;
	inverseScale._22 = 1.0f / this->compositionScaleY;

	this->swapChain->SetMatrixTransform(&inverseScale);

	D2D1_BITMAP_PROPERTIES1 bitmapProperties =
		BitmapProperties1(
			D2D1_BITMAP_OPTIONS_TARGET | D2D1_BITMAP_OPTIONS_CANNOT_DRAW,
			PixelFormat(DXGI_FORMAT_B8G8R8A8_UNORM, D2D1_ALPHA_MODE_PREMULTIPLIED),
			dipsPerInch * this->compositionScaleX,
			dipsPerInch * this->compositionScaleY
			);

	// Direct2D needs the DXGI version of the back-buffer surface pointer.
	ComPtr<IDXGISurface> dxgiBackBuffer;
	DX::ThrowIfFailed(
		this->swapChain->GetBuffer(0, IID_PPV_ARGS(&dxgiBackBuffer))
		);

	// Get a D2D surface from the DXGI back buffer to use as the D2D render target.
	ThrowIfFailed(
		this->d2dContext->CreateBitmapFromDxgiSurface(
			dxgiBackBuffer.Get(),
			&bitmapProperties,
			&this->d2dTargetBitmap
			)
		);

	this->d2dContext->SetDpi(dipsPerInch * this->compositionScaleX, dipsPerInch * this->compositionScaleY);
	this->d2dContext->SetTarget(this->d2dTargetBitmap.Get());
}

/// <summary>
/// Resizes the swap chain.
/// </summary>
void DirectXPanelBase::ResizeSwapChain()
{
	// Resize the swap chain buffer
	HRESULT hr = this->swapChain->ResizeBuffers(
		2,
		static_cast<UINT>(this->renderTargetWidth),
		static_cast<UINT>(this->renderTargetHeight),
		DXGI_FORMAT_B8G8R8A8_UNORM,
		0);

	// Check the result and handle errors
	if (hr == DXGI_ERROR_DEVICE_REMOVED || hr == DXGI_ERROR_DEVICE_RESET)
		OnDeviceLost();
	else ThrowIfFailed(hr); // Throw if not OK
}

/// <summary>
/// Creates the new instance of swap chain.
/// </summary>
void DirectXPanelBase::CreateSwapChain()
{
	// Create swap chain description
	DXGI_SWAP_CHAIN_DESC1 swapChainDesc = { 0 };
	swapChainDesc.Width = static_cast<UINT>(this->renderTargetWidth);      // Match the size of the panel.
	swapChainDesc.Height = static_cast<UINT>(this->renderTargetHeight);
	swapChainDesc.Format = DXGI_FORMAT_B8G8R8A8_UNORM;                  // This is the most common swap chain format.
	swapChainDesc.Stereo = false;
	swapChainDesc.SampleDesc.Count = 1;                                 // Don't use multi-sampling.
	swapChainDesc.SampleDesc.Quality = 0;
	swapChainDesc.BufferUsage = DXGI_USAGE_RENDER_TARGET_OUTPUT;
	swapChainDesc.BufferCount = 2;                                      // Use double buffering to enable flip.
	swapChainDesc.SwapEffect = DXGI_SWAP_EFFECT_FLIP_SEQUENTIAL;        // All Windows Store applications must use this SwapEffect.
	swapChainDesc.Flags = 0;
	swapChainDesc.AlphaMode = this->alphaMode;

	// Get underlying DXGI Device from D3D Device.
	ComPtr<IDXGIDevice1> dxgiDevice;
	ThrowIfFailed(
		this->d3dDevice.As(&dxgiDevice));

	// Get adapter.
	ComPtr<IDXGIAdapter> dxgiAdapter;
	ThrowIfFailed(
		dxgiDevice->GetAdapter(&dxgiAdapter));

	// Get factory.
	ComPtr<IDXGIFactory2> dxgiFactory;
	ThrowIfFailed(
		dxgiAdapter->GetParent(IID_PPV_ARGS(&dxgiFactory)));

	ComPtr<IDXGISwapChain1> swapChain;
	// Create swap chain.
	ThrowIfFailed(
		dxgiFactory->CreateSwapChainForComposition(
			this->d3dDevice.Get(),
			&swapChainDesc,
			nullptr,
			&swapChain
			));
	swapChain.As(&this->swapChain);

	// Ensure that DXGI does not queue more than one frame at a time. This both reduces 
	// latency and ensures that the application will only render after each VSync, minimizing 
	// power consumption.
	ThrowIfFailed(
		dxgiDevice->SetMaximumFrameLatency(1));

	// Assign swap chain to the panel
	// This must be done in the UI thread
	this->Dispatcher->RunAsync(CoreDispatcherPriority::Normal, ref new DispatchedHandler([=]()
	{
		//Get backing native interface for SwapChainPanel.
		ComPtr<ISwapChainPanelNative> panelNative;
		ThrowIfFailed(
			reinterpret_cast<IUnknown*>(this)->QueryInterface(IID_PPV_ARGS(&panelNative)));

		// Associate swap chain with SwapChainPanel. This must be done on the UI thread.
		ThrowIfFailed(
			panelNative->SetSwapChain(this->swapChain.Get()));

	}, CallbackContext::Any));
}

/// <summary>
/// Presents the rendered scene to the device panel.
/// </summary>
void DirectXPanelBase::Present()
{
	// Instantiate present parameters
	DXGI_PRESENT_PARAMETERS parameters = { 0 };
	parameters.DirtyRectsCount = 0;
	parameters.pDirtyRects = nullptr;
	parameters.pScrollRect = nullptr;
	parameters.pScrollOffset = nullptr;

	// Present and retrieve result
	HRESULT hr = S_OK;
	hr = this->swapChain->Present1(1, 0, &parameters);

	// Check the result and handle errors
	if (hr == DXGI_ERROR_DEVICE_REMOVED || hr == DXGI_ERROR_DEVICE_RESET)
		OnDeviceLost();
	else ThrowIfFailed(hr); // Throw if not OK
}

/// <summary>
/// Called when device is suspending.
/// </summary>
/// <param name="sender">The sender.</param>
/// <param name="e">The device suspending arguments.</param>
void DirectXPanelBase::OnSuspending(Object^ sender, SuspendingEventArgs^ e)
{
	critical_section::scoped_lock lock(this->criticalSection);

	// Retrieve DXGI device
	ComPtr<IDXGIDevice3> dxgiDevice;
	this->d3dDevice.As(&dxgiDevice);

	// Hints to the driver that the application is entering an idle state and that its memory can be used temporarily for other applications.
	dxgiDevice->Trim();
}

/// <summary>
/// Called when device size changed.
/// </summary>
/// <param name="sender">The sender.</param>
/// <param name="e">The size changed event arguments.</param>
void DirectXPanelBase::OnSizeChanged(Object^ sender, SizeChangedEventArgs^ e)
{
	// Ignore if size didn't change
	if (this->width == e->NewSize.Width && this->height == e->NewSize.Height)
		return;
	
	critical_section::scoped_lock lock(this->criticalSection);

	// Store values so they can be accessed from a background thread.
	this->width = max(e->NewSize.Width, 1.0f);
	this->height = max(e->NewSize.Height, 1.0f);

	// Recreate size-dependent resources when the panel's size changes.
	this->CreateSizeDependentResources();
}

/// <summary>
/// Called when device composition scale changed.
/// </summary>
/// <param name="sender">The sender.</param>
/// <param name="args">The arguments.</param>
void DirectXPanelBase::OnCompositionScaleChanged(SwapChainPanel ^sender, Object ^args)
{
	// Ignore if scale didn't change
	if (this->compositionScaleX == this->CompositionScaleX && 
		this->compositionScaleY == this->CompositionScaleY)
		return;
	
	critical_section::scoped_lock lock(this->criticalSection);

	// Store values so they can be accessed from a background thread.
	this->compositionScaleX = this->CompositionScaleX;
	this->compositionScaleY = this->CompositionScaleY;

	// Recreate size-dependent resources when the composition scale changes.
	this->CreateSizeDependentResources();	
}

/// <summary>
/// Called when graphics device is lost.
/// </summary>
void DirectXPanelBase::OnDeviceLost()
{
	// Mark as loading
	this->isLoadingComplete = false;

	// Release swap chain
	this->swapChain = nullptr;

	// Ensure the rendering state has been released
	this->ReleaseDependentObjects();

	// Release context and device
	this->d2dContext = nullptr;
	this->d2dDevice = nullptr;	

	// Recreate device resources
	this->CreateDeviceResources();
	this->CreateSizeDependentResources();
}

/// <summary>
/// Releases the dependent objects.
/// </summary>
void DirectXPanelBase::ReleaseDependentObjects()
{
	// Make sure the rendering state has been released
	this->d3dContext->OMSetRenderTargets(0, nullptr, nullptr);
	this->d3dContext->Flush();
	this->d2dContext->SetTarget(nullptr);
	this->d2dTargetBitmap = nullptr;
}

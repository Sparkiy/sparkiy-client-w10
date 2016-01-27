//*********************************************************
// 
//  DirectXPanelBase.h
//  for Sparkiy 
// 
//*********************************************************

#pragma once
#include "pch.h"
#include <concrt.h>

namespace sparkiy_Engine_Graphics_DirectX
{	
	/// <summary>
	/// Base class for a SwapChainPanle-based DirectX rendering surface 
	/// to be used inside UWP XAML application.
	/// </summary>
	[Windows::Foundation::Metadata::WebHostHidden]
	public ref class DirectXPanelBase : public Windows::UI::Xaml::Controls::SwapChainPanel {
	protected private:		
		/// <summary>
		/// Initializes a new instance of the <see cref="DirectXPanelBase"/> class.
		/// </summary>
		DirectXPanelBase();
		

		/// <summary>
		/// Creates the device independent resources.
		/// </summary>
		virtual void CreateDeviceIndependentResources();
		
		/// <summary>
		/// Creates the device resources.
		/// </summary>
		virtual void CreateDeviceResources();
		
		/// <summary>
		/// Creates the size dependent resources.
		/// </summary>
		virtual void CreateSizeDependentResources();
		
		/// <summary>
		/// Resizes the swap chain.
		/// </summary>
		virtual void ResizeSwapChain();
		
		/// <summary>
		/// Creates the new instance of swap chain.
		/// </summary>
		virtual void CreateSwapChain();

		
		/// <summary>
		/// Called when graphics device is lost.
		/// </summary>
		virtual void OnDeviceLost();
		
		/// <summary>
		/// Releases the dependent objects.
		/// </summary>
		virtual void ReleaseDependentObjects();
		
		/// <summary>
		/// Called when device size changed.
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="e">The size changed event arguments.</param>
		virtual void OnSizeChanged(Platform::Object^ sender, Windows::UI::Xaml::SizeChangedEventArgs^ e);
		
		/// <summary>
		/// Called when device composition scale changed.
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="args">The arguments.</param>
		virtual void OnCompositionScaleChanged(Windows::UI::Xaml::Controls::SwapChainPanel ^sender, Platform::Object ^args);
		
		/// <summary>
		/// Called when device is suspending.
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="e">The device suspending arguments.</param>
		virtual void OnSuspending(Platform::Object^ sender, Windows::ApplicationModel::SuspendingEventArgs^ e);
		
		/// <summary>
		/// Called when device is resuming.
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="args">The arguments.</param>
		virtual void OnResuming(Platform::Object^ sender, Platform::Object^ args) { };

		
		/// <summary>
		/// Renders the scene.
		/// </summary>
		virtual void Render() { };
		
		/// <summary>
		/// Presents the rendered scene to the device panel.
		/// </summary>
		virtual void Present();


		// Device variables
		Microsoft::WRL::ComPtr<ID3D11Device1> d3dDevice;
		Microsoft::WRL::ComPtr<ID3D11DeviceContext1> d3dContext;
		Microsoft::WRL::ComPtr<IDXGISwapChain2> swapChain;

		// Drawing factory and context
		Microsoft::WRL::ComPtr<ID2D1Factory2> d2dFactory;
		Microsoft::WRL::ComPtr<ID2D1Device> d2dDevice;
		Microsoft::WRL::ComPtr<ID2D1DeviceContext> d2dContext;
		Microsoft::WRL::ComPtr<ID2D1Bitmap1> d2dTargetBitmap;

		// Color 
		D2D1_COLOR_F backgroundColor;
		DXGI_ALPHA_MODE alphaMode;

		// Status
		bool isLoadingComplete;

		// Lock object
		Concurrency::critical_section criticalSection;

		// Size variables
		float renderTargetHeight;
		float renderTargetWidth;
		float compositionScaleX;
		float compositionScaleY;
		float height;
		float width;
	};
}

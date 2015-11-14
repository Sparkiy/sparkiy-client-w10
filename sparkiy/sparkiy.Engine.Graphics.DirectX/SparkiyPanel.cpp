//*********************************************************
// 
//  SparkiyPanel.cpp
//  for Sparkiy 
// 
//*********************************************************

#pragma once
#include "pch.h"
#include "SparkiyPanel.h"
#include "DirectXHelper.h"

#include <DirectXMath.h>
#include <DirectXColors.h>
#include <math.h>
#include <ppltasks.h>
#include <windows.ui.xaml.media.dxinterop.h>

using namespace Microsoft::WRL;
using namespace Platform;
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
using namespace sparkiy_Engine_Graphics_DirectX;

/// <summary>
/// Initializes an instance of the <see cref="SparkiyPanel"/> class.
/// </summary>
SparkiyPanel::SparkiyPanel()
{
	critical_section::scoped_lock(this->criticalSection);
	this->CreateDeviceIndependentResources();
	this->CreateDeviceResources();
	this->CreateSizeDependentResources();
}

/// <summary>
/// Finalizes an instance of the <see cref="SparkiyPanel"/> class.
/// </summary>
SparkiyPanel::~SparkiyPanel()
{
	this->StopRenderLoop();
}

/// <summary>
/// Starts the render loop.
/// </summary>
void SparkiyPanel::StartRenderLoop()
{
	// If the animation render loop is already running then do not start another thread.
	if (this->renderLoopWorker != nullptr && 
		this->renderLoopWorker->Status == AsyncStatus::Started)
		return;

	// Create a task that will be run on a background thread.
	auto workItemHandler = ref new WorkItemHandler([this](IAsyncAction ^ action)
	{
		// Calculate the updated frame and render once per vertical blanking interval.
		while (action->Status == AsyncStatus::Started)
		{
			// Update timer and render
			this->timer.Tick([&]()
			{
				critical_section::scoped_lock lock(this->criticalSection);
				Render();
			});

			// Halt the thread until the next vertical blank sync is reached.
			// This ensures the application isn't updating and rendering faster than the display can refresh,
			// which would unnecessarily spend extra CPU and GPU resources.  This helps improve battery life.
			this->dxgiOutput->WaitForVBlank();
		}
	});

	// Run task on a dedicated high priority background thread.
	this->renderLoopWorker = ThreadPool::RunAsync(
		workItemHandler, WorkItemPriority::High, WorkItemOptions::TimeSliced);
}

/// <summary>
/// Stops the render loop.
/// </summary>
void SparkiyPanel::StopRenderLoop()
{
	// Cancel the asynchronous task and let the render thread exit.
	this->renderLoopWorker->Cancel();
}

/// <summary>
/// Renders the frame.
/// </summary>
void SparkiyPanel::Render()
{
	// Don't render if panel is loading or timer has not ticked yet
	if (!this->isLoadingComplete || this->timer.GetFrameCount() == 0)
		return;

	this->d2dContext->BeginDraw();
	this->d2dContext->Clear(this->backgroundColor);

	// Set up simple tic-tac-toe game board.
	float horizontalSpacing = this->renderTargetWidth / 3.0f;
	float verticalSpacing = this->renderTargetHeight / 3.0f;

	// Since the unit mode is set to pixels in CreateDeviceResources(), here we scale the line thickness by the composition scale so that elements 
	// are rendered in the same position but larger as you zoom in. Whether or not the composition scale should be factored into the size or position 
	// of elements depends on the application's scenario.
	float lineThickness = this->compositionScaleX * 2.0f;
	float strokeThickness = this->compositionScaleX * 4.0f;

	// Draw grid lines.
	this->d2dContext->DrawLine(Point2F(horizontalSpacing, 0), Point2F(horizontalSpacing, this->renderTargetHeight), this->strokeBrush.Get(), lineThickness);
	this->d2dContext->DrawLine(Point2F(horizontalSpacing * 2, 0), Point2F(horizontalSpacing * 2, this->renderTargetHeight), this->strokeBrush.Get(), lineThickness);
	this->d2dContext->DrawLine(Point2F(0, verticalSpacing), Point2F(this->renderTargetWidth, verticalSpacing), this->strokeBrush.Get(), lineThickness);
	this->d2dContext->DrawLine(Point2F(0, verticalSpacing * 2), Point2F(this->renderTargetWidth, verticalSpacing * 2), this->strokeBrush.Get(), lineThickness);

	// Draw center circle.
	this->d2dContext->DrawEllipse(Ellipse(Point2F(this->renderTargetWidth / 2.0f, this->renderTargetHeight / 2.0f), horizontalSpacing / 2.0f - strokeThickness, verticalSpacing / 2.0f - strokeThickness), this->strokeBrush.Get(), strokeThickness);

	// Draw top left X.
	this->d2dContext->DrawLine(Point2F(0, 0), Point2F(horizontalSpacing - lineThickness, verticalSpacing - lineThickness), this->strokeBrush.Get(), strokeThickness);
	this->d2dContext->DrawLine(Point2F(horizontalSpacing - lineThickness, 0), Point2F(0, verticalSpacing - lineThickness), this->strokeBrush.Get(), strokeThickness);

	this->d2dContext->EndDraw();

	// Present the rendered content
	this->Present();
}

void SparkiyPanel::CreateDeviceResources()
{
	DirectXPanelBase::CreateDeviceResources();

	// Retrieve DXGIOutput representing the main adapter output.
	ComPtr<IDXGIFactory1> dxgiFactory;
	ThrowIfFailed(
		CreateDXGIFactory1(IID_PPV_ARGS(&dxgiFactory))
		);

	ComPtr<IDXGIAdapter> dxgiAdapter;
	ThrowIfFailed(
		dxgiFactory->EnumAdapters(0, &dxgiAdapter)
		);

	ThrowIfFailed(
		dxgiAdapter->EnumOutputs(0, &this->dxgiOutput)
		);

	this->d2dContext->CreateSolidColorBrush(ColorF(ColorF::Black), &this->strokeBrush);

	// Set D2D's unit mode to pixels so that drawing operation units are interpreted in pixels rather than DIPS. 
	this->d2dContext->SetUnitMode(D2D1_UNIT_MODE::D2D1_UNIT_MODE_PIXELS);

	this->isLoadingComplete = true;
}

void SparkiyPanel::OnDeviceLost()
{
	// Handle device lost, then re-render.
	DirectXPanelBase::OnDeviceLost();
	Render();
}

void SparkiyPanel::OnSizeChanged(Platform::Object^ sender, SizeChangedEventArgs^ e)
{
	// Process SizeChanged event, then re-render at the new size.
	DirectXPanelBase::OnSizeChanged(sender, e);
	Render();
}

void SparkiyPanel::OnCompositionScaleChanged(SwapChainPanel ^sender, Object ^args)
{
	// Process CompositionScaleChanged event, then re-render at the new scale.
	DirectXPanelBase::OnCompositionScaleChanged(sender, args);
	Render();
}

void SparkiyPanel::OnResuming(Object^ sender, Object^ args)
{
	// Ensure content is rendered when the application is resumed.
	Render();
}

//*********************************************************
// 
//  SparkiyPanel.h
//  for Sparkiy 
// 
//*********************************************************

#pragma once
#include "pch.h"
#include "DirectXPanelBase.h"
#include "StepTimer.h"
#include "ShaderStructures.h"

namespace sparkiy_Engine_Graphics_DirectX
{
	[Windows::Foundation::Metadata::WebHostHiddenAttribute]
	public ref class SparkiyPanel sealed : public sparkiy_Engine_Graphics_DirectX::DirectXPanelBase 
	{
	public:
		/// <summary>
		/// Initializes an instance of the <see cref="SparkiyPanel"/> class.
		/// </summary>
		SparkiyPanel();
		
		/// <summary>
		/// Starts the render loop.
		/// </summary>
		void StartRenderLoop();
		
		/// <summary>
		/// Stops the render loop.
		/// </summary>
		void StopRenderLoop();

	private protected:
		/// <summary>
		/// Renders the frame.
		/// </summary>
		virtual void Render() override;

		virtual void CreateDeviceResources() override;
		//virtual void CreateSizeDependentResources() override;

		virtual void OnDeviceLost() override;
		virtual void OnSizeChanged(Platform::Object^ sender, Windows::UI::Xaml::SizeChangedEventArgs^ e) override;
		virtual void OnCompositionScaleChanged(Windows::UI::Xaml::Controls::SwapChainPanel ^sender, Platform::Object ^args) override;
		virtual void OnResuming(Platform::Object^ sender, Platform::Object^ args) override;

		Microsoft::WRL::ComPtr<ID2D1SolidColorBrush> strokeBrush;

		Microsoft::WRL::ComPtr<IDXGIOutput> dxgiOutput;
		Windows::Foundation::IAsyncAction^ renderLoopWorker;
		DX::StepTimer timer;

	private:
		/// <summary>
		/// Finalizes an instance of the <see cref="SparkiyPanel"/> class.
		/// </summary>
		~SparkiyPanel();
	};
}
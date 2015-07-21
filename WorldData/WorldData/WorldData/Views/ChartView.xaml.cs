﻿using System;
using System.Globalization;
using System.Linq;
using Infragistics.XF.Controls;
using Xamarin.Forms;

namespace WorldData
{
    public partial class ChartView : ContentView
    {

        public static readonly BindableProperty DataProperty =
                BindableProperty.Create<ChartView, QuandlInfoData>(p => p.Data, null, propertyChanged: OnDataChanged);

        private static void OnDataChanged(BindableObject bindable, QuandlInfoData oldvalue, QuandlInfoData newvalue)
        {
            ((ChartView)bindable).OnDataChanged(oldvalue, newvalue);
        }

        public QuandlInfoData Data
        {
            get { return (QuandlInfoData)GetValue(DataProperty); }
            set { SetValue(DataProperty, value); }
        }

        public void OnDataChanged(QuandlInfoData oldValue, QuandlInfoData newValue)
        {
            PopulateChart(newValue);
        }


        public static readonly BindableProperty ShowOverlayProperty =
                BindableProperty.Create<ChartView, bool>(p => p.ShowOverlay, false, propertyChanged: OnShowOverlayChanged);

        private static void OnShowOverlayChanged(BindableObject bindable, bool oldvalue, bool newvalue)
        {
            ((ChartView)bindable).OnShowOverlayChanged(oldvalue, newvalue);
        }

        public bool ShowOverlay
        {
            get { return (bool)GetValue(DataProperty); }
            set { SetValue(DataProperty, value); }
        }

        public void OnShowOverlayChanged(bool oldValue, bool newValue)
        {
            if (newValue)
            {
                overlay.Opacity = 0.5;
                downloadingData.Text = "Downloading..";
            }
            else
            {
                overlay.Opacity = 0;
                downloadingData.Text = string.Empty;

            }
        }


        private void PopulateChart(QuandlInfoData newValue)
        {
            lineChart.Series[0].ItemsSource = newValue;
            var axis = lineChart.Axes.OfType<CategoryXAxis>().First();
            axis.ItemsSource = newValue;
            
        }

        public ChartView()
        {
            InitializeComponent();
            OnShowOverlayChanged(true, false);
            transformationsPicker.SelectedIndexChanged += (sender, args) =>
            {

                var selectedItem = transformationsPicker.Items[transformationsPicker.SelectedIndex];
                if (TransformationsChanged != null)
                    TransformationsChanged(this, selectedItem);
            };

            yAxis.FormatLabel += yAxis_FormatLabel;
            xAxis.FormatLabel += xAxis_FormatLabel;
        }

        string xAxis_FormatLabel(object sender, object item)
        {
            var value = (item as QuandlInfoDataItem).Date;
            return value.ToString("yyyy-MMM");
        }

        string yAxis_FormatLabel(object sender, object item)
        {
            var value = (double)item;
            if (value > 999)
                return value.ToString("#,##0,K", CultureInfo.InvariantCulture);
            else
                return value.ToString(".0#", CultureInfo.InvariantCulture);
        }

        public event EventHandler<string> TransformationsChanged;
        public event EventHandler<string> FrequencyChanged;


        private void Options_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            var selectedItem = frequencyOptions.Items[frequencyOptions.SelectedIndex];
            if (FrequencyChanged != null)
                FrequencyChanged(this, selectedItem);
        }
    }
}

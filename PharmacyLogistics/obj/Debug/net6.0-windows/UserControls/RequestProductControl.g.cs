﻿#pragma checksum "..\..\..\..\UserControls\RequestProductControl.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "4E178460C3E339768BE1528DC1904664ACB62FA9"
//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан программой.
//     Исполняемая версия:4.0.30319.42000
//
//     Изменения в этом файле могут привести к неправильной работе и будут потеряны в случае
//     повторной генерации кода.
// </auto-generated>
//------------------------------------------------------------------------------

using MaterialDesignThemes.Wpf;
using MaterialDesignThemes.Wpf.Converters;
using MaterialDesignThemes.Wpf.Transitions;
using PharmacyLogistics.UserControls;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Controls.Ribbon;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;


namespace PharmacyLogistics.UserControls {
    
    
    /// <summary>
    /// RequestProductControl
    /// </summary>
    public partial class RequestProductControl : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector {
        
        
        #line 23 "..\..\..\..\UserControls\RequestProductControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label ReqProduct_Label;
        
        #line default
        #line hidden
        
        
        #line 24 "..\..\..\..\UserControls\RequestProductControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.StackPanel EditReqProd_StackPanel;
        
        #line default
        #line hidden
        
        
        #line 26 "..\..\..\..\UserControls\RequestProductControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button MinusAmount_Button;
        
        #line default
        #line hidden
        
        
        #line 27 "..\..\..\..\UserControls\RequestProductControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox Amount_TextBox;
        
        #line default
        #line hidden
        
        
        #line 28 "..\..\..\..\UserControls\RequestProductControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button PlusAmount_Button;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "7.0.3.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/PharmacyLogistics;component/usercontrols/requestproductcontrol.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\UserControls\RequestProductControl.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "7.0.3.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.ReqProduct_Label = ((System.Windows.Controls.Label)(target));
            return;
            case 2:
            this.EditReqProd_StackPanel = ((System.Windows.Controls.StackPanel)(target));
            return;
            case 3:
            this.MinusAmount_Button = ((System.Windows.Controls.Button)(target));
            
            #line 26 "..\..\..\..\UserControls\RequestProductControl.xaml"
            this.MinusAmount_Button.Click += new System.Windows.RoutedEventHandler(this.MinusAmount_Button_Click);
            
            #line default
            #line hidden
            return;
            case 4:
            this.Amount_TextBox = ((System.Windows.Controls.TextBox)(target));
            
            #line 27 "..\..\..\..\UserControls\RequestProductControl.xaml"
            this.Amount_TextBox.TextChanged += new System.Windows.Controls.TextChangedEventHandler(this.Amount_TextBox_TextChanged);
            
            #line default
            #line hidden
            return;
            case 5:
            this.PlusAmount_Button = ((System.Windows.Controls.Button)(target));
            
            #line 28 "..\..\..\..\UserControls\RequestProductControl.xaml"
            this.PlusAmount_Button.Click += new System.Windows.RoutedEventHandler(this.PlusAmount_Button_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}


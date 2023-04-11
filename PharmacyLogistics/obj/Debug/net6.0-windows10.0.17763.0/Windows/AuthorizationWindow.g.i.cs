﻿#pragma checksum "..\..\..\..\Windows\AuthorizationWindow.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "6E068EB0B3B70C3823B44D543BE811F7D4799A52"
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
using PharmacyLogistics.Windows;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Controls.Ribbon;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms.Integration;
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


namespace PharmacyLogistics.Windows {
    
    
    /// <summary>
    /// AuthorizationWindow
    /// </summary>
    public partial class AuthorizationWindow : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 22 "..\..\..\..\Windows\AuthorizationWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox Login_TextBox;
        
        #line default
        #line hidden
        
        
        #line 24 "..\..\..\..\Windows\AuthorizationWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox Password_TextBox;
        
        #line default
        #line hidden
        
        
        #line 25 "..\..\..\..\Windows\AuthorizationWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.PasswordBox Password_PasswordBox;
        
        #line default
        #line hidden
        
        
        #line 26 "..\..\..\..\Windows\AuthorizationWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button ShowHidePassword_Button;
        
        #line default
        #line hidden
        
        
        #line 27 "..\..\..\..\Windows\AuthorizationWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Image ShowHidePassword_Image;
        
        #line default
        #line hidden
        
        
        #line 29 "..\..\..\..\Windows\AuthorizationWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button LogIn_Button;
        
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
            System.Uri resourceLocater = new System.Uri("/PharmacyLogistics;component/windows/authorizationwindow.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\Windows\AuthorizationWindow.xaml"
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
            this.Login_TextBox = ((System.Windows.Controls.TextBox)(target));
            return;
            case 2:
            this.Password_TextBox = ((System.Windows.Controls.TextBox)(target));
            
            #line 24 "..\..\..\..\Windows\AuthorizationWindow.xaml"
            this.Password_TextBox.TextChanged += new System.Windows.Controls.TextChangedEventHandler(this.Password_TextBox_TextChanged);
            
            #line default
            #line hidden
            return;
            case 3:
            this.Password_PasswordBox = ((System.Windows.Controls.PasswordBox)(target));
            
            #line 25 "..\..\..\..\Windows\AuthorizationWindow.xaml"
            this.Password_PasswordBox.PasswordChanged += new System.Windows.RoutedEventHandler(this.Password_PasswordBox_PasswordChanged);
            
            #line default
            #line hidden
            return;
            case 4:
            this.ShowHidePassword_Button = ((System.Windows.Controls.Button)(target));
            
            #line 26 "..\..\..\..\Windows\AuthorizationWindow.xaml"
            this.ShowHidePassword_Button.Click += new System.Windows.RoutedEventHandler(this.ShowHidePassword_Button_Click);
            
            #line default
            #line hidden
            return;
            case 5:
            this.ShowHidePassword_Image = ((System.Windows.Controls.Image)(target));
            return;
            case 6:
            this.LogIn_Button = ((System.Windows.Controls.Button)(target));
            
            #line 29 "..\..\..\..\Windows\AuthorizationWindow.xaml"
            this.LogIn_Button.Click += new System.Windows.RoutedEventHandler(this.LogIn_Button_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}


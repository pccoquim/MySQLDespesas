﻿#pragma checksum "..\..\Frm_020102_FamiliasEdit.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "1021C9D94E9D9C1254EDF743E9CEB93E1D6B80B4E0B9DA4787954135F0477E75"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using GestaoDom;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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


namespace GestaoDom {
    
    
    /// <summary>
    /// Frm_020102_FamiliasEdit
    /// </summary>
    public partial class Frm_020102_FamiliasEdit : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 12 "..\..\Frm_020102_FamiliasEdit.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label lbl_Cod;
        
        #line default
        #line hidden
        
        
        #line 13 "..\..\Frm_020102_FamiliasEdit.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txt_Cod;
        
        #line default
        #line hidden
        
        
        #line 14 "..\..\Frm_020102_FamiliasEdit.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label lbl_Descr;
        
        #line default
        #line hidden
        
        
        #line 15 "..\..\Frm_020102_FamiliasEdit.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txt_Descr;
        
        #line default
        #line hidden
        
        
        #line 16 "..\..\Frm_020102_FamiliasEdit.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label lbl_Status;
        
        #line default
        #line hidden
        
        
        #line 17 "..\..\Frm_020102_FamiliasEdit.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox cbx_Status;
        
        #line default
        #line hidden
        
        
        #line 19 "..\..\Frm_020102_FamiliasEdit.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btn_Save;
        
        #line default
        #line hidden
        
        
        #line 20 "..\..\Frm_020102_FamiliasEdit.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btn_Close;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/GestaoDom;component/frm_020102_familiasedit.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\Frm_020102_FamiliasEdit.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.lbl_Cod = ((System.Windows.Controls.Label)(target));
            return;
            case 2:
            this.txt_Cod = ((System.Windows.Controls.TextBox)(target));
            return;
            case 3:
            this.lbl_Descr = ((System.Windows.Controls.Label)(target));
            return;
            case 4:
            this.txt_Descr = ((System.Windows.Controls.TextBox)(target));
            
            #line 15 "..\..\Frm_020102_FamiliasEdit.xaml"
            this.txt_Descr.LostFocus += new System.Windows.RoutedEventHandler(this.Txt_Descr_LostFocus);
            
            #line default
            #line hidden
            
            #line 15 "..\..\Frm_020102_FamiliasEdit.xaml"
            this.txt_Descr.KeyDown += new System.Windows.Input.KeyEventHandler(this.Txt_Descr_KeyDown);
            
            #line default
            #line hidden
            return;
            case 5:
            this.lbl_Status = ((System.Windows.Controls.Label)(target));
            return;
            case 6:
            this.cbx_Status = ((System.Windows.Controls.ComboBox)(target));
            return;
            case 7:
            this.btn_Save = ((System.Windows.Controls.Button)(target));
            
            #line 19 "..\..\Frm_020102_FamiliasEdit.xaml"
            this.btn_Save.Click += new System.Windows.RoutedEventHandler(this.Btn_Save_Click);
            
            #line default
            #line hidden
            return;
            case 8:
            this.btn_Close = ((System.Windows.Controls.Button)(target));
            
            #line 20 "..\..\Frm_020102_FamiliasEdit.xaml"
            this.btn_Close.Click += new System.Windows.RoutedEventHandler(this.Btn_Close_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}


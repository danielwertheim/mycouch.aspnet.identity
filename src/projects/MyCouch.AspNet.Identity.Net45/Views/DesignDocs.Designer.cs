﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34003
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MyCouch.AspNet.Identity.Views {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class DesignDocs {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal DesignDocs() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("MyCouch.AspNet.Identity.Views.DesignDocs", typeof(DesignDocs).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to {
        ///   &quot;_id&quot;: &quot;_design/userstore&quot;,
        ///   &quot;language&quot;: &quot;javascript&quot;,
        ///   &quot;views&quot;: {
        ///       &quot;usernames&quot;: {
        ///           &quot;map&quot;: &quot;function(doc) {\n  if(doc.$doctype &amp;&amp; doc.$doctype === &apos;identityuser&apos;) {\n    emit(doc.userName, null);\n  }\n}&quot;
        ///       },
        ///       &quot;loginprovider_providerkey&quot;: {
        ///           &quot;map&quot;: &quot;function(doc) {\n  if(doc.$doctype &amp;&amp; doc.$doctype === &apos;identityuser&apos; &amp;&amp; doc.logins) {\n    for(var i = 0, l = doc.logins.length; i &lt; l; i++)\n      emit([doc.logins[i].loginProvider, doc.logins[i].providerK [rest of string was truncated]&quot;;.
        /// </summary>
        public static string UserStore {
            get {
                return ResourceManager.GetString("UserStore", resourceCulture);
            }
        }
    }
}
﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace crypto.Core.Recources {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "16.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Strings {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Strings() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("crypto.Core.Recources.Strings", typeof(Strings).Assembly);
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
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The range doesn&apos;t fit in the array.
        /// </summary>
        internal static string ArrayExtension_SetRange_The_range_doesn_t_fit_in_the_array {
            get {
                return ResourceManager.GetString("ArrayExtension_SetRange_The_range_doesn_t_fit_in_the_array", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to File not found.
        /// </summary>
        internal static string Vault_AddFileAsync_File_not_found {
            get {
                return ResourceManager.GetString("Vault_AddFileAsync_File_not_found", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Decrypted file was not found.
        /// </summary>
        internal static string Vault_EliminateExtracted_Decrypted_file_was_not_found {
            get {
                return ResourceManager.GetString("Vault_EliminateExtracted_Decrypted_file_was_not_found", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to File already exists in Vault.
        /// </summary>
        internal static string Vault_FileAlreadyExists_File_already_exists_in_Vault {
            get {
                return ResourceManager.GetString("Vault_FileAlreadyExists_File_already_exists_in_Vault", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Couldn&apos;t take out file..
        /// </summary>
        internal static string Vault_RemoveFile_Couldn_t_take_out_file_ {
            get {
                return ResourceManager.GetString("Vault_RemoveFile_Couldn_t_take_out_file_", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Argument isn&apos;t a name.
        /// </summary>
        internal static string Vault_RenameFile_Argument_isn_t_a_name {
            get {
                return ResourceManager.GetString("Vault_RenameFile_Argument_isn_t_a_name", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Magic number doesn&apos;t match.
        /// </summary>
        internal static string VaultHeaderReader_ReadFrom_Magic_number_doesn_t_match {
            get {
                return ResourceManager.GetString("VaultHeaderReader_ReadFrom_Magic_number_doesn_t_match", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Couldn&apos;t find vault file for path {0}.
        /// </summary>
        internal static string VaultPaths_VaultPaths_Couldn_t_find_vault_file_for_path__0_ {
            get {
                return ResourceManager.GetString("VaultPaths_VaultPaths_Couldn_t_find_vault_file_for_path__0_", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Password wasn&apos;t able to be verified.
        /// </summary>
        internal static string VaultReaderWriter_ReadFromConfig_Password_wasn_t_able_to_be_verified {
            get {
                return ResourceManager.GetString("VaultReaderWriter_ReadFromConfig_Password_wasn_t_able_to_be_verified", resourceCulture);
            }
        }
    }
}

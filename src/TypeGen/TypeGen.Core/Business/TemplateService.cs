﻿using TypeGen.Core.Storage;
using TypeGen.Core.Utils;

namespace TypeGen.Core.Business
{
    /// <summary>
    /// Contains logic for filling templates with data
    /// </summary>
    internal class TemplateService : ITemplateService
    {
        // dependencies

        private readonly IInternalStorage _internalStorage;

        private string _enumTemplate;
        private string _enumValueTemplate;
        private string _classTemplate;
        private string _classPropertyTemplate;
        private string _classPropertyWithDefaultValueTemplate;
        private string _interfaceTemplate;
        private string _interfacePropertyTemplate;
        private string _importTemplate;
        private string _indexTemplate;
        private string _indexExportTemplate;
        private string _headingTemplate;

        public GeneratorOptions GeneratorOptions { get; set; }

        public TemplateService(IInternalStorage internalStorage)
        {
            _internalStorage = internalStorage;
            LoadTemplates();
        }

        private void LoadTemplates()
        {
            _enumTemplate = _internalStorage.GetEmbeddedResource("TypeGen.Core.Templates.Enum.tpl");
            _enumValueTemplate = _internalStorage.GetEmbeddedResource("TypeGen.Core.Templates.EnumValue.tpl");
            _classTemplate = _internalStorage.GetEmbeddedResource("TypeGen.Core.Templates.Class.tpl");
            _classPropertyTemplate = _internalStorage.GetEmbeddedResource("TypeGen.Core.Templates.ClassProperty.tpl");
            _classPropertyWithDefaultValueTemplate = _internalStorage.GetEmbeddedResource("TypeGen.Core.Templates.ClassPropertyWithDefaultValue.tpl");
            _interfaceTemplate = _internalStorage.GetEmbeddedResource("TypeGen.Core.Templates.Interface.tpl");
            _interfacePropertyTemplate = _internalStorage.GetEmbeddedResource("TypeGen.Core.Templates.InterfaceProperty.tpl");
            _importTemplate = _internalStorage.GetEmbeddedResource("TypeGen.Core.Templates.Import.tpl");
            _indexTemplate = _internalStorage.GetEmbeddedResource("TypeGen.Core.Templates.Index.tpl");
            _indexExportTemplate = _internalStorage.GetEmbeddedResource("TypeGen.Core.Templates.IndexExport.tpl");
            _headingTemplate = _internalStorage.GetEmbeddedResource("TypeGen.Core.Templates.Heading.tpl");
        }

        public string FillClassTemplate(string imports, string name, string extends, string properties, string customHead, string customBody, string fileHeading = null)
        {
            if (fileHeading == null) fileHeading = _headingTemplate;
            
            return ReplaceSpecialChars(_classTemplate)
                .Replace(GetTag("imports"), imports)
                .Replace(GetTag("name"), name)
                .Replace(GetTag("extends"), extends)
                .Replace(GetTag("properties"), properties)
                .Replace(GetTag("customHead"), customHead)
                .Replace(GetTag("customBody"), customBody)
                .Replace(GetTag("fileHeading"), fileHeading);
        }

        public string FillClassPropertyWithDefaultValueTemplate(string accessor, string name, string type, string defaultValue)
        {
            return ReplaceSpecialChars(_classPropertyWithDefaultValueTemplate)
                .Replace(GetTag("accessor"), accessor)
                .Replace(GetTag("name"), name)
                .Replace(GetTag("type"), type)
                .Replace(GetTag("defaultValue"), defaultValue);
        }

        public string FillClassPropertyTemplate(string accessor, string name, string type)
        {
            return ReplaceSpecialChars(_classPropertyTemplate)
                .Replace(GetTag("accessor"), accessor)
                .Replace(GetTag("name"), name)
                .Replace(GetTag("type"), type);
        }

        public string FillInterfaceTemplate(string imports, string name, string extends, string properties, string customHead, string customBody, string fileHeading = null)
        {
            if (fileHeading == null) fileHeading = _headingTemplate;
            
            return ReplaceSpecialChars(_interfaceTemplate)
                .Replace(GetTag("imports"), imports)
                .Replace(GetTag("name"), name)
                .Replace(GetTag("extends"), extends)
                .Replace(GetTag("properties"), properties)
                .Replace(GetTag("customHead"), customHead)
                .Replace(GetTag("customBody"), customBody)
                .Replace(GetTag("fileHeading"), fileHeading);
        }

        public string FillInterfacePropertyTemplate(string name, string type, bool isOptional)
        {
            return ReplaceSpecialChars(_interfacePropertyTemplate)
                .Replace(GetTag("name"), name)
                .Replace(GetTag("modifier"), isOptional ? "?" : "")
                .Replace(GetTag("type"), type);
        }

        public string FillEnumTemplate(string imports, string name, string values, bool isConst, string fileHeading = null)
        {
            if (fileHeading == null) fileHeading = _headingTemplate;
            
            return ReplaceSpecialChars(_enumTemplate)
                .Replace(GetTag("imports"), imports)
                .Replace(GetTag("name"), name)
                .Replace(GetTag("values"), values)
                .Replace(GetTag("modifiers"), isConst ? " const" : "")
                .Replace(GetTag("fileHeading"), fileHeading);
        }

        public string FillEnumValueTemplate(string name, int? intValue = null, string stringValue = null)
        {
            string value = intValue != null ? intValue.ToString() : $@"""{stringValue}""";
            
            return ReplaceSpecialChars(_enumValueTemplate)
                .Replace(GetTag("name"), name)
                .Replace(GetTag("value"), value);
        }

        public string FillImportTemplate(string name, string typeAlias, string path)
        {
            string aliasText = string.IsNullOrEmpty(typeAlias) ? "" : $" as {typeAlias}";

            return ReplaceSpecialChars(_importTemplate)
                .Replace(GetTag("name"), name)
                .Replace(GetTag("aliasText"), aliasText)
                .Replace(GetTag("path"), path);
        }

        public string FillIndexTemplate(string exports)
        {
            return ReplaceSpecialChars(_indexTemplate)
                .Replace(GetTag("exports"), exports);
        }

        public string FillIndexExportTemplate(string filename)
        {
            return ReplaceSpecialChars(_indexExportTemplate)
                .Replace(GetTag("filename"), filename);
        }

        public string GetExtendsText(string name) => $" extends {name}";

        private string GetTag(string tagName) => $"$tg{{{tagName}}}";

        private string ReplaceSpecialChars(string template)
        {
            return template
                .Replace(GetTag("tab"), StringUtils.GetTabText(GeneratorOptions.TabLength))
                .Replace(GetTag("quot"), GeneratorOptions.SingleQuotes ? "'" : "\"");
        }
    }
}

(function() {
    require.config({
        paths: {
            "EmailTemplateLookupGalleryElementMF": Terrasoft.getFileContentUrl("CrtDynamicContent7x", "src/js/marketing-campaign-ng-elements/marketing-campaign.js"),
            "EmailTemplateLookupGalleryElementStyles": Terrasoft.getFileContentUrl("CrtDynamicContent7x", "src/js/marketing-campaign-ng-elements/styles.css")
		},
        shim: {
            "EmailTemplateLookupGalleryElementMF": {
                deps: ["css-ltr!EmailTemplateLookupGalleryElementStyles"]
            }
        }
    });

    define("EmailTemplateLookupGalleryElement", ["mf!EmailTemplateLookupGalleryElementMF"], function(module) {
        return module;
    });
})();
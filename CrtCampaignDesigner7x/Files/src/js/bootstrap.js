(function () {
	require.config({
		paths: {
			"SimpleDiagram": Terrasoft.getFileContentUrl("CrtCampaignDesigner7x", "src/js/cytoscape.min.js"),
			"CampaignDesignerComponentMF": Terrasoft.getFileContentUrl("CrtCampaignDesigner7x", "src/js/campaign-designer-component/campaign-designer.js"),
			"CampaignDesignerComponentStyles": Terrasoft.getFileContentUrl("CrtCampaignDesigner7x", "src/js/campaign-designer-component/styles.css"),
			"SvgRenderer": Terrasoft.getFileContentUrl("CrtCampaignDesigner7x", "src/js/canvg.min.js"),
			"CampaignGallery": Terrasoft.getFileContentUrl("CrtCampaignDesigner7x", "src/js/campaign-gallery-element/mf/campaign-gallery-element.js"),
			"CampaignGalleryStyles": Terrasoft.getFileContentUrl("CrtCampaignDesigner7x", "src/js/campaign-gallery-element/mf/styles.css"),
		},
		shim: {
			"SimpleDiagram": {
				deps: [""]
			},
			"CampaignDesignerComponentMF": {
				deps: ["css-ltr!CampaignDesignerComponentStyles"]
			},
			"SvgRenderer": {
				deps: [""]
			}
		}
	});

	define("CampaignDesignerComponent", ["mf!CampaignDesignerComponentMF"], function (module) {
		return module;
	});
})();

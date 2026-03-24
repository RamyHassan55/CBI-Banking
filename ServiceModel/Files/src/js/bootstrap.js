(function() {
	require.config({
		paths: {
			"ServiceModelNetworkComponentMF": Terrasoft.getFileContentUrl("ServiceModel", "src/js/service-model-network-component/service-model-network/service-model-network.js"),
			"ServiceModelNetworkComponentStyles": Terrasoft.getFileContentUrl("ServiceModel", "src/js/service-model-network-component/service-model-network/styles.css"),
		},
		shim: {
			"ServiceModelNetworkComponentMF": {
				deps: ["css-ltr!ServiceModelNetworkComponentStyles"]
			}
		}
	});
	define("ServiceModelNetworkComponent", ["mf!ServiceModelNetworkComponentMF"], function(module) {
		return module;
	});
})();
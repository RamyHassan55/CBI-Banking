(function() {
	require.config({
		paths: {
			"TermCalculationComponent": Terrasoft.getFileContentUrl("CaseService", "src/js/term-calculation-component/mf/term-calculation.js"),
			"TermCalculationComponentStyles": Terrasoft.getFileContentUrl("CaseService", "src/js/term-calculation-component/mf/styles.css"),
		},
		shim: {
		}
	});
})();
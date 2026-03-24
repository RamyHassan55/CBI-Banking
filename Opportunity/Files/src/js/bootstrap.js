(function() {
    require.config({
        paths: {
            "ConfidenceLevelWidgetComponent": Terrasoft.getFileContentUrl("Opportunity", "src/js/confidence-level-widget/mf/confidence-level-widget.js"),
            "ConfidenceLevelWidgetComponentStyles": Terrasoft.getFileContentUrl("Opportunity", "src/js/confidence-level-widget/mf/styles.css"),
        },
        shim: {
        }
    });
})();

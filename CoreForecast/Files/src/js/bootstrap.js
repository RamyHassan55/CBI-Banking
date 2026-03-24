(function() {
    require.config({
        paths: {
            "ForecastComponent": Terrasoft.getFileContentUrl("CoreForecast", "src/js/forecast-component/mf/forecast-component.js"),
            "ForecastComponentStyles": Terrasoft.getFileContentUrl("CoreForecast", "src/js/forecast-component/mf/styles.css")
        },
        shim: {
        }
    });
})();

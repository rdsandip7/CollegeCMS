// "Students by intake" stacked bar chart. Colours come from CSS tokens so the chart
// follows the light/dark theme, and it is redrawn when the theme toggle fires.
(() => {
    const canvas = document.getElementById("intakeChart");
    const source = document.getElementById("intakeData");
    if (!canvas || !source || !window.Chart) return;

    const data = JSON.parse(source.textContent);
    let chart;

    const token = (name) => getComputedStyle(document.documentElement).getPropertyValue(name).trim();

    function render() {
        chart?.destroy();
        const surface = token("--app-surface");
        const muted = token("--app-muted");
        const grid = token("--app-border");
        const last = data.series.length - 1;

        chart = new Chart(canvas, {
            type: "bar",
            data: {
                labels: data.labels,
                datasets: data.series.map((s, i) => ({
                    label: s.name,
                    data: s.values,
                    backgroundColor: token(`--series-${i + 1}`),
                    hoverBackgroundColor: token(`--series-${i + 1}`),
                    borderColor: surface,
                    borderWidth: { top: 2 },
                    borderSkipped: false,
                    borderRadius: i === last ? { topLeft: 4, topRight: 4 } : 0,
                    maxBarThickness: 64,
                })),
            },
            options: {
                maintainAspectRatio: false,
                interaction: { mode: "index", intersect: false },
                scales: {
                    x: { stacked: true, grid: { display: false }, border: { color: grid }, ticks: { color: muted } },
                    y: {
                        stacked: true,
                        beginAtZero: true,
                        grid: { color: grid },
                        border: { display: false },
                        ticks: { color: muted, precision: 0 },
                    },
                },
                plugins: {
                    legend: { display: false },
                    tooltip: {
                        backgroundColor: surface,
                        titleColor: token("--bs-body-color"),
                        bodyColor: token("--bs-body-color"),
                        borderColor: grid,
                        borderWidth: 1,
                        padding: 10,
                        boxPadding: 4,
                        usePointStyle: true,
                        callbacks: {
                            footer: (items) => `Total: ${items.reduce((sum, item) => sum + item.parsed.y, 0)} students`,
                        },
                    },
                },
            },
        });
    }

    render();
    document.addEventListener("themechange", render);
})();

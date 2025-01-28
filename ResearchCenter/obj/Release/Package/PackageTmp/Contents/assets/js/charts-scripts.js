
function barChart() {
    fetch('/WebHome/GetChartData')
        .then(response => response.json())
        .then(data => {
            var ctx = document.getElementById('barChart').getContext('2d');
            var myChart = new Chart(ctx, {
                type: 'bar', // Change this to 'line', 'pie', etc. to use different chart types
                data: {
                    labels: data.labels,
                    datasets: data.datasets
                },
                options: {
                    responsive: true,
                        plugins: {
                            legend: {
                                labels: {
                                    backgroundColor: 'black',
                                    color: 'gray', // Change legend text color
                                    font: {
                                        size: 14
                                    }
                                }
                            }
                        },
                    scales: {
                        y: {
                            beginAtZero: true, // Start the Y-axis at zero
                            ticks: {
                                stepSize: 10, // Set the step size between ticks
                                callback: function (value) {
                                    return value + ' P'; // Append text to each tick label
                                }
                            },
                            grid: {
                                color: 'rgba(200, 200, 200, 0.2)', // Color of grid lines
                                borderColor: 'rgba(200, 200, 200, 0.8)' // Color of border
                            }
                        }
                    }
                }
            });
        })
        .catch(error => console.error('Error fetching chart data:', error));
};
function lineChart() {
    fetch('/WebHome/GetChartData')
        .then(response => response.json())
        .then(data => {
            var ctx = document.getElementById('lineChart').getContext('2d');
            var myChart = new Chart(ctx, {
                type: 'line', // Change this to 'line', 'pie', etc. to use different chart types
                data: {
                    labels: data.labels,
                    datasets: data.datasets
                },
                options: {
                    responsive: true,
                    scales: {
                        y: {
                            beginAtZero: true,
                            ticks: {
                                stepSize: 10, // Set the step size between ticks
                                callback: function (value) {
                                    return value + ' P'; // Append text to each tick label
                                }
                            },
                            grid: {
                                color: 'rgba(200, 200, 200, 0.2)', // Color of grid lines
                                borderColor: 'rgba(200, 200, 200, 0.8)' // Color of border
                            }
                        }
                    }
                }
            });
        })
        .catch(error => console.error('Error fetching chart data:', error));
};
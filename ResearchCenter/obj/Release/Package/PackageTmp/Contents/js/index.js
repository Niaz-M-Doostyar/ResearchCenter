$(function () {

    "use strict";

    // chart 1
    
            var ctx = document.getElementById('chart1').getContext('2d');

            var myChart = new Chart(ctx, {
                type: 'line',
                data: {
                    labels: ["ss", "Febaa", "Massr", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct"],
                    datasets: [{
                        label: 'New Visitor',
                        data: [3, 3, 8, 5, 7, 4, 6, 4, 6, 3],
                        backgroundColor: '#fff',
                        borderColor: "transparent",
                        pointRadius: "0",
                        borderWidth: 3
                    }, {
                        label: 'Old Visitor',
                        data: [7, 5, 14, 7, 12, 6, 10, 6, 11, 5],
                        backgroundColor: "rgba(255, 255, 255, 0.25)",
                        borderColor: "transparent",
                        pointRadius: "0",
                        borderWidth: 1
                    }]
                },
                options: {
                    maintainAspectRatio: false,
                    legend: {
                        display: false,
                        labels: {
                            fontColor: '#ddd',
                            boxWidth: 40
                        }
                    },
                    tooltips: {
                        displayColors: false
                    },
                    scales: {
                        xAxes: [{
                            ticks: {
                                beginAtZero: true,
                                fontColor: '#ddd'
                            },
                            gridLines: {
                                display: true,
                                color: "rgba(221, 221, 221, 0.08)"
                            },
                        }],
                        yAxes: [{
                            ticks: {
                                beginAtZero: true,
                                fontColor: '#ddd'
                            },
                            gridLines: {
                                display: true,
                                color: "rgba(221, 221, 221, 0.08)"
                            },
                        }]
                    }

                }
            });

        });

});
import { Chart, registerables } from "https://cdn.jsdelivr.net/npm/chart.js@4.4.2/+esm";
Chart.register(...registerables);

import { getAccountMetrics, getEmailMetrics, getEventMetrics, getUserCount } from "../api/admin/metrics.js";

// The entire configuration for the email activity metrics chart
const emailActivityChartOptions = {
    type: "line",
    data: {
        labels: ["January", "February", "March", "April", "May", "June", "July"],
        datasets: [{
            label: "Emails Sent",
            data: [65, 59, 80, 81, 56, 55, 40],
            borderColor: "rgba(0, 123, 255, 1)",
            fill: false
        }, {
            label: "# of Users",
            data: [28, 48, 40, 19, 86, 27, 90],
            borderColor: "rgba(40, 167, 69, 1)",
            fill: false
        }]
    },
    options: {
        responsive: true,
        scales: {
            yAxes: [{
                title: {
                    display: true,
                    text: "# of Results"
                },
                ticks: {
                    min: 0,
                    beginAtZero: true
                },
                gridLines: {
                    display: true,
                    color: "rgba(255,99,164,0.2)"
                }
            }],
            xAxes: [{
                title: {
                    display: true,
                    text: "Month"
                },
                ticks: {
                    min: 0,
                    beginAtZero: true
                },
                gridLines: {
                    display: false
                }
            }]
        },
    }
};

// The entire configuration for the user account metrics chart
const userAccountChartOptions = {
    type: "line",
    data: {
        labels: ["January", "February", "March", "April", "May", "June", "July"],
        datasets: [{
            label: "Accounts Created",
            data: [65, 59, 80, 81, 56, 55, 40],
            borderColor: "rgba(0, 123, 255, 1)",
            fill: false
        }, {
            label: "Accounts Deleted",
            data: [28, 48, 40, 19, 86, 27, 90],
            borderColor: "rgba(40, 167, 69, 1)",
            fill: false
        }]
    },
    options: {
        responsive: true,
        scales: {
            yAxes: [{
                title: {
                    display: true,
                    text: "# of Results"
                },
                ticks: {
                    min: 0,
                    beginAtZero: true
                },
                gridLines: {
                    display: true,
                    color: "rgba(255,99,164,0.2)"
                }
            }],
            xAxes: [{
                title: {
                    display: true,
                    text: "Month"
                },
                ticks: {
                    min: 0,
                    beginAtZero: true
                },
                gridLines: {
                    display: false
                }
            }]
        },
    }
};

// The entire configuration for the event activity metrics chart
const eventActivityChartOptions = {
    type: "bar",
    data: {
        labels: ["January", "February", "March", "April", "May", "June", "July"],
        datasets: [{
            label: "Events Created",
            data: [65, 59, 80, 81, 56, 55, 40],
            backgroundColor: [
                "rgba(0, 123, 255, 0.2)",
                "rgba(0, 123, 255, 0.2)",
                "rgba(0, 123, 255, 0.2)",
                "rgba(0, 123, 255, 0.2)",
                "rgba(0, 123, 255, 0.2)",
                "rgba(0, 123, 255, 0.2)",
                "rgba(0, 123, 255, 0.2)"
            ],
            borderColor: [
                "rgba(0, 123, 255, 1)",
                "rgba(0, 123, 255, 1)",
                "rgba(0, 123, 255, 1)",
                "rgba(0, 123, 255, 1)",
                "rgba(0, 123, 255, 1)",
                "rgba(0, 123, 255, 1)",
                "rgba(0, 123, 255, 1)"
            ],
            borderWidth: 1
        }]
    },
    options: {
        responsive: true,
        legend: {
            display: false
        },
        scales: {
            yAxes: [{
                title: {
                    display: true,
                    text: "# of Searches"
                },
                ticks: {
                    min: 0,
                    beginAtZero: true
                },
                gridLines: {
                    display: true,
                    color: "rgba(255,99,164,0.2)"
                }
            }],
            xAxes: [{
                title: {
                    display: true,
                    text: "Month"
                },
                ticks: {
                    min: 0,
                    beginAtZero: true
                },
                gridLines: {
                    display: false
                }
            }]
        },
    }
};

let emailActivityChart = null;
let userAccountChart = null;
let eventActivityChart = null;

document.addEventListener("DOMContentLoaded", async function () {
    await updateEmailActivityChart();
    await updateUserAccountChart();
    await updateEventActivityChart();

    document.getElementById("email-activity-metrics-chart-select").addEventListener("change", updateEmailActivityChart);
    document.getElementById("user-account-metrics-chart-select").addEventListener("change", updateUserAccountChart);
    document.getElementById("event-activity-metrics-chart-select").addEventListener("change", updateEventActivityChart);
});

async function updateEmailActivityChart() {
    var emailCtx = document.getElementById("email-activity-metrics-chart").getContext("2d");
    var time = document.getElementById("email-activity-metrics-chart-select").value;
    time = getTime(time);

    var data = await getEmailMetrics(time);
    var userCount = await getUserCount();
    emailActivityChartOptions.data.labels = data.labels;
    emailActivityChartOptions.data.datasets[0].data = data.buckets;
    emailActivityChartOptions.data.datasets[1].data = [];
    data.labels.forEach(() => { emailActivityChartOptions.data.datasets[1].data.push(userCount); });

    emailActivityChart?.destroy();
    emailActivityChart = new Chart(emailCtx, emailActivityChartOptions);
}

async function updateUserAccountChart() {
    var userCtx = document.getElementById("user-account-metrics-chart").getContext("2d");
    var time = document.getElementById("user-account-metrics-chart-select").value;
    time = getTime(time);

    var data = await getAccountMetrics(time);
    console.log(data);
    userAccountChartOptions.data.labels = data.labels;
    userAccountChartOptions.data.datasets[0].data = [];
    userAccountChartOptions.data.datasets[1].data = [];

    data.buckets.forEach((bucket) => {
        userAccountChartOptions.data.datasets[0].data.push(+bucket["item1"]);
        userAccountChartOptions.data.datasets[1].data.push(+bucket["item2"]);
    });

    userAccountChart?.destroy();
    userAccountChart = new Chart(userCtx, userAccountChartOptions);
}

async function updateEventActivityChart() {
    var eventCtx = document.getElementById("event-activity-metrics-chart").getContext("2d");
    var time = document.getElementById("event-activity-metrics-chart-select").value;
    time = getTime(time);

    var data = await getEventMetrics(time);
    console.log(data);
    eventActivityChartOptions.data.labels = data.labels;
    eventActivityChartOptions.data.datasets[0].data = data.buckets;

    eventActivityChart?.destroy();
    eventActivityChart = new Chart(eventCtx, eventActivityChartOptions);
}

function getTime(time) {
    switch (time) {
        case "all": time = -1; break;
        case "today": time = 0; break;
        case "yesterday": time = 1; break;
        case "week": time = 7; break;
        case "month": time = 30; break;
        case "year": time = 365; break;
    }

    return time;
}
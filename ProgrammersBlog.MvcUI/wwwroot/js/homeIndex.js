$(document).ready(function () {
    $('#articlesTable').DataTable({
        order: [[4, 'desc']]
    });

    // Chart.js - Dinamik veri ile çalışacak şekilde düzenlendi

    // İlk chart - Dinamik Bar Chart
    // Canvas elementinin yüklenmesini bekle
    setTimeout(function() {
        $.get('/Admin/Article/GetAllByViewCount/?isAscending=false&takeSize=10')
        .done(function(data) {
            console.log("AJAX başarılı:", data);
            
            if (!data) {
                console.error("Veri boş geldi!");
                return;
            }
            
            const articleResult = jQuery.parseJSON(data);
            
            if (!articleResult || !articleResult.$values) {
                console.error("Geçersiz veri formatı:", articleResult);
                return;
            }
            
            // Tekrar eden makaleleri filtrele ve benzersiz hale getir
            const uniqueArticles = [];
            const seenTitles = new Set();
            
            articleResult.$values.forEach(article => {
                if (!seenTitles.has(article.Title)) {
                    seenTitles.add(article.Title);
                    uniqueArticles.push(article);
                }
            });
            
            console.log("Benzersiz makaleler:", uniqueArticles.length);
            
            var viewCountCanvas = document.getElementById("viewCountChart");
            
            // Canvas elementinin varlığını kontrol et
            if (!viewCountCanvas) {
                console.error("viewCountChart canvas elementi bulunamadı!");
                return;
            }
            
            var viewCountContext = viewCountCanvas.getContext("2d");
            
            var viewCountChart = new Chart(viewCountContext, {
                type: 'bar',
                data: {
                    labels: uniqueArticles.map(article => {
                        // Başlığı 25 karakterle sınırla ve daha kısa tut
                        return article.Title.length > 25 ? article.Title.substring(0, 25) + '...' : article.Title;
                    }),
                    datasets: [
                        {
                            label: 'View Count',
                            data: uniqueArticles.map(article => article.ViewsCount),
                            backgroundColor: '#3498db',
                            borderColor: '#2980b9',
                            borderWidth: 2,
                            borderRadius: 8,
                            borderSkipped: false,
                            hoverBackgroundColor: '#2980b9',
                            hoverBorderWidth: 3,
                            hoverBorderColor: '#1f5f8b'
                        },
                        {
                            label: 'Comment Count',
                            data: uniqueArticles.map(article => article.CommentCount),
                            backgroundColor: '#2ecc71',
                            borderColor: '#27ae60',
                            borderWidth: 2,
                            borderRadius: 8,
                            borderSkipped: false,
                            hoverBackgroundColor: '#27ae60',
                            hoverBorderWidth: 3,
                            hoverBorderColor: '#1e8449'
                        }
                    ]
                },
                options: {
                    responsive: true,
                    maintainAspectRatio: false,
                    plugins: {
                        legend: {
                            position: 'bottom',
                            labels: {
                                font: {
                                    size: 14,
                                    weight: 'bold'
                                },
                                color: '#2C3E50',
                                usePointStyle: true,
                                padding: 15
                            }
                        }
                    },
                    scales: {
                        y: {
                            beginAtZero: true,
                            grid: {
                                color: 'rgba(0,0,0,0.05)',
                                drawBorder: false
                            },
                            ticks: {
                                color: '#7f8c8d',
                                font: {
                                    size: 11,
                                    weight: '500'
                                },
                                padding: 5
                            }
                        },
                        x: {
                            grid: {
                                display: false
                            },
                            ticks: {
                                color: '#7f8c8d',
                                font: {
                                    size: 9,
                                    weight: '500'
                                },
                                maxRotation: 45,
                                minRotation: 0,
                                padding: 5
                            }
                        }
                    },
                    interaction: {
                        mode: 'nearest',
                        intersect: false
                    },
                    layout: {
                        padding: {
                            top: 5,
                            right: 0,
                            bottom: 5,
                            left: 0
                        }
                    }
                }
            });
        })
        .fail(function(xhr, status, error) {
            console.error("AJAX hatası:", error);
            console.error("Status:", status);
            console.error("Response:", xhr.responseText);
        });
    }, 100); // 100ms gecikme
});

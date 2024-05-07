//This file is responsible for the layout page of the website. It contains the javascript for the onhover effect of the SVGs in the layout page.
//When a user clicks on an SVG link, the SVG will be highlighted and the index of the SVG will be stored in the session storage.
//This will allow the SVG to remain highlighted even after the page is refreshed.
document.addEventListener('DOMContentLoaded', function() {
    var navbarState = sessionStorage.getItem('navbarState');

    // If there's a saved state, apply it
    if (navbarState === 'minimized') {
        document.querySelector('.custom-bottom-navbar').style.display = 'none';
        document.querySelector('.minimized-navbar').style.display = 'flex';
    } else {
        document.querySelector('.custom-bottom-navbar').style.display = 'flex';
        document.querySelector('.minimized-navbar').style.display = 'none';
    }
    
    document.getElementById('close-navbar').addEventListener('click', function () {
        document.querySelector('.custom-bottom-navbar').style.display = 'none';
        document.querySelector('.minimized-navbar').style.display = 'flex';
        // Save the state to sessionStorage
        sessionStorage.setItem('navbarState', 'minimized');
    });
    
    document.getElementById('close-navbar-2').addEventListener('click', function () {
        document.querySelector('.custom-bottom-navbar').style.display = 'flex';
        document.querySelector('.minimized-navbar').style.display = 'none';
        // Save the state to sessionStorage
        sessionStorage.setItem('navbarState', 'expanded');
    });
});


// document.querySelectorAll('.onhover svg').forEach(function (svg, index) {
//     svg.addEventListener('click', function (event) {
//         // Remove the 'clicked' class from all SVGs
//         document.querySelectorAll('.onhover svg.clicked').forEach(function (clickedSvg) {
//             clickedSvg.classList.remove('clicked');
//         });

//         // Add the 'clicked' class to the clicked SVG
//         this.classList.add('clicked');

//         // Store the index of the clicked SVG in sessionStorage
//         sessionStorage.setItem('clickedSvgIndex', index);
//     });

//     // If this SVG was the last one clicked before the page refresh, add the 'clicked' class to it
//     if (sessionStorage.getItem('clickedSvgIndex') == index) {
//         svg.classList.add('clicked');
//     }
// });

let searchTimer;

export function Init() {
    document.getElementById('SearchQuery').addEventListener('input', function(){
        PostSearch(this.value);
    });
}

function PostSearch(searchQuery) {
    clearTimeout(searchTimer);
    searchTimer = setTimeout(function() {
        fetch('/Watched/Search', {
            method: 'post',
            body: new URLSearchParams({ searchQuery: searchQuery }) 
        })
        .then(function(response) {
            return response.text()
        })
        .then((response) => {
            let container = document.getElementById('SelectContainer');
            container.innerHTML = response;
            container.classList.remove('hidden');
        });
    }, 1000);
}

export function Select(tmdbId) {
    fetch('/Watched/Select', {
        method: 'post',
        body: new URLSearchParams({ tmdbId: tmdbId }) 
    })
    .then(function(response) {
        return response.text()
    })
    .then((response) => {
        let container = document.getElementById('Main');
        container.innerHTML = response;
    });
}
let searchTimer;

export function Init() {
    document.getElementById('SearchQuery').addEventListener('input', function(){
        PostSearch(this.value);
    });
}

function PostSearch(searchQuery) {
    clearTimeout(searchTimer);
    searchTimer = setTimeout(function() {
        fetch('/Read/Search', {
            method: 'post',
            body: new URLSearchParams({ searchQuery: searchQuery }) 
        })
        .then(function(response) {
            return response.text();
        })
        .then((response) => {
            let container = document.getElementById('SelectContainer');
            container.innerHTML = response;
            container.classList.remove('hidden');
        });
    }, 1000);
}

export function Select(goodreadsId) {
    fetch('/Read/Select', {
        method: 'post',
        body: new URLSearchParams({ goodreadsId: goodreadsId }) 
    })
    .then(function(response) {
        return response.text();
    })
    .then((response) => {
        let container = document.getElementById('Main');
        container.innerHTML = response;
    });
}

export function SelectForUpdate(readId) {
    fetch('/Read/SelectForUpdate', {
        method: 'post',
        body: new URLSearchParams({ readId: readId }) 
    })
    .then(function(response) {
        return response.text();
    })
    .then((response) => {
        let container = document.getElementById('Main');
        container.innerHTML = response;
    });
}
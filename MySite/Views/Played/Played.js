let searchTimer;

export function Init() {
    document.getElementById('SearchQuery').addEventListener('input', function(){
        PostSearch(this.value);
    });
}

function PostSearch(searchQuery) {
    clearTimeout(searchTimer);
    searchTimer = setTimeout(function() {
        fetch('/Played/Search', {
            method: 'post',
            body: new URLSearchParams({
                searchQuery: searchQuery
            })
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

export function Select(igdbId) {
    fetch('/Played/Select', {
        method: 'post',
        body: new URLSearchParams({ igdbId: igdbId }) 
    })
    .then(function(response) {
        return response.text();
    })
    .then((response) => {
        let container = document.getElementById('Main');
        container.innerHTML = response;
    });
}

export function SelectForUpdate(playedId) {
    fetch('/Played/SelectForUpdate', {
        method: 'post',
        body: new URLSearchParams({ playedId: playedId }) 
    })
    .then(function(response) {
        return response.text();
    })
    .then((response) => {
        let container = document.getElementById('Main');
        container.innerHTML = response;
    });
}

export function Delete(readId) {
    fetch('/Played/Delete', {
        method: 'post',
        redirect: 'follow',
        body: new URLSearchParams({ readId: readId }) 
    })
    .then(response => {
        if (response.status === 200) {
            window.location = "/Played";
        }
    });
}
let searchTimer;

export function Init() {
    document.getElementById('SearchQuery').addEventListener('input', function(){
        PostSearch(this.value);
    });
}

function PostSearch(searchQuery) {
    clearTimeout(searchTimer);
    searchTimer = setTimeout(function() {
        var watchedType = document.querySelector('input[name="WatchedType"]:checked').value;
        fetch('/Watched/Search', {
            method: 'post',
            body: new URLSearchParams({
                searchQuery: searchQuery,
                watchedType: watchedType
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

export function Select(tmdbId, watchedType) {
    fetch('/Watched/Select', {
        method: 'post',
        body: new URLSearchParams({
            tmdbId: tmdbId,
            watchedType: watchedType
        }) 
    })
    .then(function(response) {
        return response.text()
    })
    .then((response) => {
        let container = document.getElementById('Main');
        container.innerHTML = response;
    });
}

export function SelectForUpdate(watchedId) {
    fetch('/Watched/SelectForUpdate', {
        method: 'post',
        body: new URLSearchParams({ watchedId: watchedId }) 
    })
    .then(function(response) {
        return response.text();
    })
    .then((response) => {
        let container = document.getElementById('Main');
        container.innerHTML = response;
    });
}

export function Delete(watchedId) {
    fetch('/Watched/Delete', {
        method: 'post',
        redirect: 'follow',
        body: new URLSearchParams({ watchedId: watchedId }) 
    })
    .then(response => {
        if (response.status === 200) {
            window.location = "/Watched";
        }
    });
}
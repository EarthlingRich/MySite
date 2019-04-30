let addSearchFormTimer;

export function Init() {
    document.getElementById('SearchQuery').addEventListener('input', function(){
        PostAddSearchForm(this.value);
    });
}

function PostAddSearchForm(searchQuery) {
    clearTimeout(addSearchFormTimer);
    addSearchFormTimer = setTimeout(function() {
            fetch('/Add/SelectMovie', {
            method: 'post',
            body: new URLSearchParams({ searchQuery: searchQuery }) 
        })
        .then(function(response) {
            return response.text()
        })
        .then((response) => {
            let container = document.getElementById('SelectContainer');
            container.innerHTML = response;
        });
    }, 1000);
}
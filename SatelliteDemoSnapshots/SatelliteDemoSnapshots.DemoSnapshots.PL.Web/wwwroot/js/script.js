const webApiUri = 'https://localhost:44323/DemoSnapshot/';
const webApiSearchUri = 'https://localhost:44323/DemoSnapshot/?query=';
const homeUri = 'https://localhost:44398/';
const createUri = 'https://localhost:44398/create.html';
const editUri = 'https://localhost:44398/edit.html';
const deleteUri = 'https://localhost:44398/delete.html';
const Satellites = Object.freeze({
    Kanopus: "Kanopus",
    BS: "BS",
    Meteor: "Meteor",
    Sentinel: "Sentinel",
    KOMPSAT: "KOMPSAT",
    Resurs: "Resurs"
})

document.addEventListener('DOMContentLoaded', (event) => {
    var id;

    switch (window.location.origin + window.location.pathname) {
        case homeUri:
            {
                var query = new URLSearchParams(window.location.search).get("query");
                buildGetPageAsync(query);
                break;
            }
        case createUri:
            {
                buildCreatePage();
                break;
            }
        case editUri:
            {
                id = new URLSearchParams(window.location.search).get("itemId");
                buildEditPageAsync(id);
                break;
            }
        case deleteUri:
            {
                id = new URLSearchParams(window.location.search).get("itemId");
                buildDeletePageAsync(id);
                break;
            }
        default:
            break;
    }
})

function loadIndexPage() {
    location.href = homeUri;
}

function loadCreatePage() {
    location.href = createUri;
}

function loadEditPage(id) {
    location.href = editUri + `?itemId=${id}`;
}

function loadDeletePage(id) {
    location.href = deleteUri + `?itemId=${id}`;
}

async function fetchAsync(url) {
    let response = await fetch(url);

    if (response.ok) {
        return response.json();
    }
    else {
        alert(`HTTP error: ${response.status}`);
    }
}

function postNewDemoSnapshot() {
    const formData = new FormData(document.getElementById('create-panel'));
    var object = {};
    formData.forEach((value, key) => {
        object[key] = value;
    });

    convertData(object);
    var json = JSON.stringify(object);

    fetch(webApiUri, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json;charset=utf-8'
        },
        body: json
    })
        .then(response => response.json())
        .then(result => alert("Response: " + JSON.stringify(result, null, 2)));
}

function putDemoSnapshot(id) {
    const formData = new FormData(document.getElementById('edit-panel'));
    var object = {};
    formData.forEach((value, key) => {
        object[key] = value;
    });

    convertData(object);
    object.id = id;

    var json = JSON.stringify(object);
    console.log(json);
    fetch(webApiUri + id, {
        method: 'PUT',
        headers: {
            'Content-Type': 'application/json;charset=utf-8'
        },
        body: json
    })
        .then(response => response.json())
        .then(result => alert("Response: " + JSON.stringify(result, null, 2)));
}

function deleteDemoSnapshot(id) {
    fetch(webApiUri + id, {
        method: 'DELETE',
    })
        .then(response => response.json())
        .then(result => alert("Response: " + JSON.stringify(result, null, 2)));
}

async function buildGetPageAsync(query) {
    let data;

    if (query) {
        data = await fetchAsync(webApiSearchUri + query);
    }
    else {
        data = await fetchAsync(webApiUri);
    }

    createTableBody(data);
}

function buildCreatePage() {
    const satellite = document.getElementById('satellite');
    addOptionsToSelectList(satellite, null);
}

async function buildEditPageAsync(id) {
    let data = await fetchAsync(webApiUri + id);
    createEditElement(data);
}

async function buildDeletePageAsync(id) {
    let data = await fetchAsync(webApiUri + id);
    createDeleteElement(data);
}

function createTableBody(data) {
    const tBody = document.getElementById('table');
    tBody.innerHTML = '';

    const button = document.createElement('button');

    data.forEach(item => {
        let tr = tBody.insertRow();

        let td1 = tr.insertCell(0);
        let idNode = document.createTextNode(item.id);
        td1.appendChild(idNode);

        let td2 = tr.insertCell(1);
        let satelliteNode = document.createTextNode(item.satellite);
        td2.appendChild(satelliteNode);

        let td3 = tr.insertCell(2);
        let shootingDateNode = document.createTextNode(toDMYDate(item.shootingDate));
        td3.appendChild(shootingDateNode);

        let td4 = tr.insertCell(3);
        let cloudinessNode = document.createTextNode('');
        if (item.cloudiness) {
            cloudinessNode = document.createTextNode(item.cloudiness + '%');
        }
        td4.appendChild(cloudinessNode);

        let td5 = tr.insertCell(4);
        let turnNode = document.createTextNode(item.turn);
        td5.appendChild(turnNode);

        let td6 = tr.insertCell(5);
        let coordinatesNode = document.createTextNode(item.coordinates);
        td6.appendChild(coordinatesNode);

        let td7 = tr.insertCell(6);
        let btnGroup = document.createElement('div');
        btnGroup.classList.add("btn-group", "px-1");
        td7.appendChild(btnGroup);

        let editButton = button.cloneNode(false);
        editButton.innerText = 'Edit';
        editButton.classList.add("btn", "btn-outline-warning", "shadow-sm");
        editButton.setAttribute('onclick', `loadEditPage(${item.id})`);
        btnGroup.appendChild(editButton);

        let deleteButton = button.cloneNode(false);
        deleteButton.innerText = 'Delete';
        deleteButton.classList.add("btn", "btn-outline-danger", "shadow-sm");
        deleteButton.setAttribute('onclick', `loadDeletePage(${item.id})`);
        btnGroup.appendChild(deleteButton);
    });
}

function convertData(object) {
    if (object.cloudiness) {
        object.cloudiness = parseFloat(object.cloudiness);
    }
    else {
        object.cloudiness = null;
    }
    if (object.turn) {
        object.turn = parseInt(object.turn, 10);
    }
}

function createEditElement(item) {
    const id = document.getElementById('id');
    id.value = item.id;
    const satellite = document.getElementById('satellite');
    addOptionsToSelectList(satellite, item);
    satellite.value = item.satellite;
    const shootingDate = document.getElementById('shootingDate');
    shootingDate.value = toYMDDate(item.shootingDate);
    const cloudiness = document.getElementById('cloudiness');
    cloudiness.value = item.cloudiness;
    const turn = document.getElementById('turn');
    turn.value = item.turn;
    const coordinates = document.getElementById('coordinates');
    coordinates.value = item.coordinates;

    document.getElementById('editButton').setAttribute('onclick', `putDemoSnapshot(${item.id})`);
}

function addOptionsToSelectList(element, item) {
    for (var [key, value] of Object.entries(Satellites)) {
        let option = document.createElement("option");
        option.value = key;
        let idValueNode = document.createTextNode(`${value.valueOf()}`);
        option.appendChild(idValueNode);

        if (item && value === item.satellite) {
            option.setAttribute('selected', "selected");
        }

        element.appendChild(option);
    }
}

function createDeleteElement(item) {
    const element = document.getElementById('element');
    element.innerHTML = '';

    for (var [key, value] of Object.entries(item)) {
        let dl = document.createElement("dl");
        dl.classList.add("row", "text-break");
        element.appendChild(dl);

        let dt = document.createElement("dt");
        dt.classList.add("col-4", "text-uppercase", "text-end");
        dl.appendChild(dt);

        let idKeyNode = document.createTextNode(`${key}:`);
        dt.appendChild(idKeyNode);

        let dd = document.createElement("dd");
        dd.classList.add("col-8");
        dl.appendChild(dd);

        let idValueNode = document.createTextNode(`${value}`);
        dd.appendChild(idValueNode);

        document.getElementById('deleteButton').setAttribute('onclick', `deleteDemoSnapshot(${item.id})`);
    }
}

function searchDemoSnapshot(event) {
    event.preventDefault();
    var query = document.getElementById('SearchInput').value;
    buildGetPageAsync(query);
}

function toYMDDate(dateString) {
    var partsArray = dateString.split(/[ ]|[.]/);

    return `${partsArray[2]}-${partsArray[1]}-${partsArray[0]}`;
}

function toDMYDate(dateString) {
    var partsArray = dateString.split(/[ ]/);

    return partsArray[0];
}
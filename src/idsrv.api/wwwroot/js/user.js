$(document).ready(function () {
    var access_token = Cookies.get("access_token");
    var userId;

    authQuery('user', 'GET', access_token, 
        function (res) {
            alert("invalid token/not logged in.");
            window.location.href = "/Login.html";
        },
        function (res) {
            userId = res.userId;
            $('#table').bootstrapTable({
                data: [res]
            });
        }
    );

    $('.revoke').click(function (e) {
        var serial = prompt("Revoke serial number:", 0);

        toggleSerial('revoke', serial, access_token);
    });

    $('.grant').click(function (e) {
        var serial = prompt("Grant serial number:", 0);

        toggleSerial('grant', serial, access_token);
    });

    $('.logout').click(function (e) {
        authQuery("logout", "POST", access_token, null, null,
            function () {
                alert("user logged out.");
                window.location.href = "/Login.html";
            }
        );
    });

    $('.delete').click(function (e) {
        authQuery("delete/" + userId, "DELETE", access_token, null, null,
            function () {
                alert("user deleted.");
                window.location.href = "/Login.html";
            }
        );
    });
});

function toggleSerial(action, serial, access_token) {
    authQuery(action + '/' + serial, "PUT", access_token, null, null,
        function (res) {
            alert(action + ' success');    
        }
    );
}

function authQuery(action, type, access_token, error, success, complete) {
    $.ajax('api/auth/' + action, {
        type: type,
        dataType: "json",
        contentType: "application/json",
        headers: {
            "Authorization": "Bearer " + access_token
        },
        error: error,
        success: success,
        complete: complete
    });
}
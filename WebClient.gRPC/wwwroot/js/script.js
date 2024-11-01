const xhr = new XMLHttpRequest();

xhr.open("GET","/GetAll");

xhr.onload = () =>{
    if(xhr.status == 200)
    {
        console.log(xhr.responseText);
    }
    else
    {
        console.log("Server response: ",xhr.responseText);
    }
}

xhr.send();
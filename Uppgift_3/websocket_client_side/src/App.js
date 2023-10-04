import './App.css';
import { useEffect, useRef, useState } from 'react';

/*
Setting up connection to the websocket server.
Setting upp the object to send to the server.
When page first load and connection is open it send the data to the server.
When message is received from the server the output value will be shown to the user.
The user can also send a string message to the server and will receive the message sent back from the server side.
*/
function App() {
  const webSocket = new WebSocket('wss://localhost:7285/ws')

  const [receivedMessages, setReceivedMessages] = useState([]);
  const message = useRef();

  var sendData = {
    "forecast": "Cloudy and 20% chance of rain",
    "temp": 12,
    "mintemp": 9,
    "maxtemp": 15
  }

  useEffect(()=>{
    webSocket.onopen = (event) => {
      webSocket.send(JSON.stringify(sendData))
     };
  }, []);

  webSocket.onmessage = (event) =>{
    setReceivedMessages([...receivedMessages, event.data]);
    receivedMessages.push(event.data);
  }

  function sendMessage() {
    webSocket.send(message.current.value);
    document.body.querySelector('input').value = "";
  }
  

  return (
    <div className="App">
        <h3>Received data from server:</h3>
        <div>{receivedMessages.map( msg =>
          <div>{ msg }</div>
        )}</div>
        <h3>Send message:</h3>
        <input ref={message}></input>
        <br></br>
        <button onClick={sendMessage}>Send</button><br></br>
    </div>
  );
}

export default App;
 
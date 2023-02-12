import { createContext, useContext, useState } from "react";
import { useEffect } from "react";
import { useRef } from "react";
import {Link, Navigate} from 'react-router-dom';
import axios from 'axios';
import { HubConnectionBuilder, HttpTransportType } from '@microsoft/signalr';
import { useNavigate,useLocation } from 'react-router-dom';

export const userContext = createContext({
  uloga:"",
  //connection:null
});

export function UserContextProvider({ children }) {

  const [email, setEmail] = useState("");   
  const [jePosetilac,setJePosetilac]=useState(true);
  const [uloga,setUloga]=useState("");
  
  
  const [ connection, setConnection ] = useState(null);

  function fja(){
    if(connection && localStorage.getItem("role")=="Korisnik")
    {
      //var user=localStorage.getItem("user");
      //connection.on("SendMessageToAll",()=> {console.log("Ne znam ni ja")})
      //ovde vracam sve kategorije korinsika koji je ulogovan i radim JoinGroup za te kategorije
      //axios.get(`https://localhost:7107/Kategorija/vratiSveKategorijeNaKojeJeKorisnikPretplacen/${user}`).then(res=>
      //{
        //res.data.map(kat=>
         // {
            
            connection.invoke('JoinGroup',localStorage.getItem("id"));
            //console.log("Kategorijeeee")
            
          //});
      //})

    }
  }

  useEffect(()=>{
   
    if(connection && localStorage.getItem("user"))
      
          {
            console.log("uso")
            connection.start().then(p=>
              {
                console.log("start")
                fja();
                
              });
            
          }    
  },[connection])

  useEffect(()=> {
      if(localStorage.getItem("user"))
      {
        if(localStorage.getItem("user")=="admin@gmail.com")
        { 
          setEmail("admin@gmail.com");
          setUloga("Admin");
          setJePosetilac(false);

        }
        else{
          setEmail(localStorage.getItem("user"));
          setUloga("Korisnik");
          setJePosetilac(false);
          const newConnection = new HubConnectionBuilder()
        .withUrl('https://localhost:5001/hubs/notif'/*, //port bi trebalo da je ovaj
        {
          withCredentials : false,
          skipNegotiation: true,
          transport: HttpTransportType.WebSockets
        }*/)  
        .withAutomaticReconnect()
        .build();
        console.log(newConnection);
        setConnection(newConnection);
          //connection.invoke('JoinGroup',localStorage.getItem("id"));


        }
      }
  
  },[])

    function logIn(email) {
        setEmail(email);
        setJePosetilac(false);
        setUloga(uloga);
        const newConnection = new HubConnectionBuilder()
        .withUrl('https://localhost:5001/hubs/notif'/*, //port bi trebalo da je ovaj
        {
          withCredentials : false,
          skipNegotiation: true,
          transport: HttpTransportType.WebSockets
        }*/)  
        .withAutomaticReconnect()
        .build();
        console.log(newConnection);
        setConnection(newConnection);
        //return <Navigate to="/Pocetna" replace />;
        if(localStorage.getItem("role")=="Korisnik")
          {
            window.location.reload();
            }
        
    }

  function logOut() {
    setEmail("");
    setJePosetilac(true);
    setUloga("");
    window.localStorage.removeItem("user");
    return <Navigate to="/" replace />;
  }

  return (
    <userContext.Provider value={{ email, jePosetilac, logIn, logOut,connection }}>   
      {children}
    </userContext.Provider>
  );
}

export function useUserContext() {
  const { email, jePosetilac, logIn, logOut,connection} = useContext(userContext);

  return useContext(userContext);
}
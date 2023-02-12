import React from 'react';
import { Navigate, useNavigate } from 'react-router-dom';
import { useState, useEffect, useRef } from "react";
import axios from 'axios';
import { useUserContext } from '../context/UserContext';
import './Pocetna.css'
import KarticaKurs from './KarticaKurs'
import { HubConnectionBuilder } from '@microsoft/signalr';

function Pocetna() {
    const [skola,setSkola]=useState("")
    const[kategorije,setKategorije]=useState([])
    const[jezici,setJezici]=useState([]) 
    const[kursevi,setKursevi]=useState([])  
    const[kategorija,setKategorija]=useState("nema")
    const[jezik,setJezik]=useState("nema")
    const {logOut,uloga}=useUserContext();
    const [notifikacije,setNotifikacije]=useState([])
    const {connection} = useUserContext();
    const [pritisnuto,setPritisnuto]=useState(false)

    

    const latestChat = useRef(null);

    latestChat.current = notifikacije;
  
    const navigate = useNavigate();

      useEffect(()=> {
        axios.get(`https://localhost:5001/Rezervacija/PreuzmiSveNotifikacijeKorisnika/${localStorage.getItem("id")}`)
        .then(res => {setNotifikacije(res.data);})
        .catch(err => {})
      },[pritisnuto])

      useEffect(()=>{
        
        axios.get(`https://localhost:5001/Rezervacija/PreuzmiSveNotifikacijeKorisnika/${localStorage.getItem("id")}`)
        .then(res => {
          setNotifikacije(res.data);
          console.log("uloga")
        })
        .catch(err => {})
      },[uloga])

    useEffect(()=>{
      console.log("provera")
      if (connection!=null)
      {
        connection.on("SendMessageToAll",(Naziv,idKorisnika)=>{
          const prop={
            Naziv,
            idKorisnika
          };
          const updatedChat = [...latestChat.current];
          updatedChat.unshift(prop);
          //console.log("stare notif "+latestChat.length)
          //console.log("updated notif "+updatedChat.length)
          /*if(updatedChat.length>latestChat.length)
          {
            setPritisnuto(false);
            //console.log("uso")
          }*/
          setNotifikacije(updatedChat);
        })
      }
     
    }, [connection])
    
    useEffect(()=> {

      axios.get('https://localhost:5001/Skola/PreuzmiSkolu')
      .then(res=>{
        setSkola(res.data)
      })
      .catch(err=>{
        console.log(err)
      })
        //FETCH ZA KATEGORIJE
      axios.get('https://localhost:5001/Kategorija/PreuzmiSveKategorije')
      .then(res=>{
        setKategorije(res.data)
      })
      .catch(err=>{
        console.log(err)
      })
      
  
      //FETCH ZA JEZIKE!!
      axios.get('https://localhost:5001/Kurs/PreuzmiSveJezike')    
      .then(res=>{
        setJezici(res.data)
      })
      .catch(err=>{
        console.log(err)
      })
      console.log(localStorage.getItem("id"))
      axios.get(`https://localhost:5001/Rezervacija/PreuzmiSveNotifikacijeKorisnika/${localStorage.getItem("id")}`)
        .then(res => {
          setNotifikacije(res.data);
        console.log("svePreuzmi");
      })
        .catch(err => {})
      
    }, [])
  
    
  
    /*useEffect(()=> {
  
      /*if(uloga=="Korisnik")    
      { 
        var user=localStorage.getItem("user")
        axios.get(`https://localhost:7107/PubSub/GetSubscriptions/${user}`)    
      .then(res=>{
        setPretplate(res.data.vesti)  //proveri
       // setVesti(res.data.vesti)
        console.log("uloga use effect")
        setProcitano(res.data.status)
      })
      .catch(err=>{
        console.log(err)
      })}*/
  
      
    //}, [uloga])
  
    function veliko(string){
      return string.charAt(0).toUpperCase()+string.slice(1);
    }
  
    function pretrazi()
    {
      if(kategorija!="nema")
      {
        console.log(kategorija);
        if (jezik!="nema")
        {
          axios.get(`https://localhost:5001/Kurs/PreuzmiSveKurseve/${kategorija}/${jezik}`)
          .then(res=>{
            setKursevi(res.data)
            console.log(res.data)
          })
          .catch(err=>{
            if(err.response.status==400)
        {
          setKursevi([])
          alert(err.response.data);
        }
        })
      }
      else
      {
       
          axios.get(`https://localhost:5001/Kurs/PreuzmiSveKurseve/${kategorija}/nema`)
          .then(res=>{
            setKursevi(res.data)
            console.log(res.data)
          })
          .catch(err=>{
            if(err.response.status==400)
            {
              setKursevi([])
              alert(err.response.data);
            }
        })
      }
    }
      else{
        console.log("IMA")
          axios.get(`https://localhost:5001/Kurs/PreuzmiSveKurseve/nema/nema`)
          .then(res=>{
            setKursevi(res.data)
            console.log(kursevi)
          })
          .catch(err=>{
            alert("Morate izabrati barem kategoriju!!")
        })
  
      }
    }
  

  
    const brisanjeVesti=async(idVest)=>{
      /*console.log(idVest);
      axios.delete(`https://localhost:7107/Vest/DeleteVest/${idVest}`)   
          .then((res) => {
              setVesti(vesti.filter((vest)=>vest.id!==idVest));
              alert("Uspesno ste obrisali vest");
          })
          .catch(err =>{
            if(err.response.status==401 || err.response.status==403)
            {   
                logOut();          
            }
          }) */
    }
  
    const dodajVest=async()=>{
      /*axios.get('https://localhost:7107/Vest/getSveVesti')
      .then(res=>{
        setVesti(res.data)
      })
      .catch(err=>{
        console.log(err)
      })*/
    }
  
    const dodajKategoriju=async()=>{
      /*axios.get('https://localhost:7107/Kategorija/getKategorije')    
      .then(res=>{
        setKategorije(res.data)
      })
      .catch(err=>{
        console.log(err)
      })*/
    }
  
    return (
      <div className='Pocetna'>
         <div className="slika"> 
         </div>
         <div className='infoDiv'>
            <h1 className='imeSkole'>{skola.naziv}</h1>
            <div className='opisSkola'>{skola.opis}</div>
          </div>
          {localStorage.getItem("role")=="Korisnik" ? (
                 <div className={pritisnuto && notifikacije.length>0 ? "divNotifikacije" : "obican"}>
                  <button type="button" className="icon-button" onClick={()=>{setPritisnuto(!pritisnuto)}}>
                    <i className="fa-solid fa-bell fa-2x"></i>
                    {notifikacije.length>0 && !pritisnuto &&<div className="icon-button__badge">{notifikacije.length}
                    </div>
                    }
                    </button>{
                     pritisnuto &&
                    <ul className='listaNot'>
                      {notifikacije.map((not)=>
                      <div className='notifDiv'>{not.naziv}</div>)}
                      </ul>
                      }
                      </div>
                ): (<></>)}
          <div className='PretragaDiv'>
            <>
            <div className="PretragaForma">
            <div className="Programi">Pretrazi kurseve</div>

            <div className="red">
                <label>Kategorija:</label>
                <select name='Kategorija' className='KategorijaSelect' onChange={e=>setKategorija(e.target.value)}>
                <option value=' ' selected disabled hidden className='sivo'>Izaberite kategoriju</option>
               
                {
                    kategorije.map((d) => 
                    <option key={d.id} value={d.id}>{veliko(d.uzrast)}</option>)
                    //<option key={d} value={d}> {veliko(d)} </option>
                    
                }
                </select> 
            </div>

            <div className="red">
                <label>Jezik:</label>
                <select name='Kategorija' className='KategorijaSelect' onChange={e=>setJezik(e.target.value)}>
                <option value=' ' selected disabled hidden className='sivo'>Izaberite jezik</option>
                {
                    jezici.map((d) => 
                    <option key={d} value={d}>{veliko(d)}</option>)   
                }
                </select> 
            </div>

            
            <button onClick={()=>pretrazi()} className="dugmePretrazi">Pretrazi</button>

        </div>
            </>
          </div>
          <ul>
              {
                  kursevi.map((el) => 
                  <div className='VestDiv'>
                    
                      <KarticaKurs key={el.id} propvest={el}  /*onDelete={brisanjeVesti}*/></KarticaKurs>
                  </div>
                 
      
                  )
              }
          </ul>
          <div className='kontaktDiv'>
            <div className='kontakt'>KONTAKT: {skola.kontakt}</div>
            <div className='kontakt'>
              LOKACIJA: {skola.lokacija}
            </div>
          </div>
        
      </div>
    );
  }
  
  export default Pocetna;
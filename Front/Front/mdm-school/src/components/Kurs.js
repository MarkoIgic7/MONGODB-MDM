import React, {Component, useEffect} from 'react';
import { useState } from "react";
import { useNavigate,useLocation } from 'react-router-dom';
import ReactDOM from "react-dom";
import {Link, Navigate, useParams} from 'react-router-dom';
import axios from 'axios';
import { useUserContext } from '../context/UserContext';
import './Pocetna.css'
import { Login } from './Login';
import './Button.css'
import DodajKursForma from './DodajKursForma.js'
import KarticaGrupa from './KarticaGrupa';

function Kurs(){
    let {id} = useParams();
    const[kurs,setKurs]=useState("")
    const navigate = useNavigate();
    const [skola,setSkola]=useState("")
    const [forma, setForma] = useState(false);
   
    const [naziv,setNaziv]=useState("")
    const [jezik,setJezik]=useState("")
    const [kOpis,setKOpis]=useState("")
    const [kateg,setKateg]=useState("")
    const [kategs,setKategs]=useState([])
    const [cena,setCena]=useState("")
    const[duziOpis,setDuziOpis]=useState("")
    
    const [grupe,setGrupe]=useState([])
    const [nazivGrupe,setNazivGrupe]=useState("nema")
    const [max,setMax]=useState("nema")
    const [trenutno,setTrenutno]=useState("nema")
    
  

    function veliko(string){
      return string.charAt(0).toUpperCase()+string.slice(1);
    }

  
      function izmeni()
      {

        axios.put(`https://localhost:5001/Kurs/IzmeniKurs/${id}/${naziv==""?kurs.naziv:naziv}/${jezik==""?kurs.jezik:jezik}/${kOpis==""?kurs.kratakOpis:kOpis}/${cena==""?kurs.cena:cena}/${duziOpis==""?kurs.duziOpis:duziOpis}`)    
        .then(res=>{
          setKurs(res.data)
          console.log('izmeni');
          setForma(!forma);
          //window.location.reload(true);
        })
        .catch(err=>{
          console.log(err)
        })
        
      }

      
    useEffect(()=> {

      axios.get('https://localhost:5001/Skola/PreuzmiSkolu')
      .then(res=>{
        setSkola(res.data)
      })
      .catch(err=>{
        console.log(err)
      })

      axios.get('https://localhost:5001/Kategorija/PreuzmiSveKategorije')
        .then(res=>{
          setKategs(res.data)
        })
        .catch(err=>{
        })

        //FETCH ZA KURS PREKO ID-A

        
        axios.get(`https://localhost:5001/Kurs/PreuzmiCeoKurs/${id}`)    
        .then(res=>{
          setKurs(res.data)
        })
        .catch(err=>{
          console.log(err)
        })
        //preuzmi grupe
        axios.get(`https://localhost:5001/Grupa/PreuzmiSveGrupe/${id}`)    
        .then(res=>{
          setGrupe(res.data)
        })
        .catch(err=>{
          console.log(err)
        })
        
    
      }, [])

      if(forma) {
        document.body.classList.add('active-modal')
      } else {
        document.body.classList.remove('active-modal')
      }

     

      
      const toggleForma = () => {
        setForma(!forma);
      };
      
    
return (
    <div className="Pocetna">
        <div className="slika"> 
         </div>
         
         <div className='infoDiv'>
          <div clasName= "izmeniDiv">
          <h1 className='imeSkole'>{kurs.naziv}</h1>
          { localStorage.getItem("role")=="Admin"?
            <button className="btnIzmeniKurs"  onClick={toggleForma} >Izmeni informacije o kursu</button>:<></>}
          </div>
          {forma && (
      <div className="modal">
        <div className="overlay"></div>
        <div className="login_modal-content_izmeni">
          
          <i className="fa-solid fa-x close-modal " onClick={toggleForma}></i>
            <div>Izmeni osnovne informacije o kursu</div>
          <div className="red">
        <label>Naziv:</label>
        <input type="text" defaultValue={kurs.naziv} name="Naziv"  className='InputIznos'  onChange={(e) => setNaziv(e.target.value)}></input>
        </div>
        <div className="red">
        <label>Jezik:</label>
        <input type="text" defaultValue={kurs.jezik} name="Naziv"  className='InputIznos'  onChange={(e) => setJezik(e.target.value)}></input>
        </div>
        <div className="red">
        <label>Kraci opis:</label>
        <textarea className="inputOpis" defaultValue={kurs.kratakOpis} type="text" placeholder='Unesite tekst'  onChange={(e) => setKOpis(e.target.value)}></textarea>
        </div>
        <div className="red">
        <label>Cena:</label>
        <input type="text" defaultValue={kurs.cena} name="Naziv"  className='InputIznos'  onChange={(e) => setCena(e.target.value)}></input>
        </div>
        <div className="red">
        <label>Duzi opis:</label>
        <textarea className="inputOpis" defaultValue={kurs.duziOpis} type="text" placeholder='Unesite tekst'   onChange={(e) => setDuziOpis(e.target.value)}></textarea>
    </div>
   
    <button onClick={()=>izmeni()} className="dugmePretrazi">Izmeni</button>

        </div>
      </div>
    )}  
            <div className='jezikDiv'>{kurs.jezik}</div>
            <div className='opisSkola'>{kurs.duziOpis}</div>
<div className='kursInformacije'>
  {
    grupe.map((el)=>
    <KarticaGrupa prop={el} kurs={kurs}></KarticaGrupa>
    )
  }
            
          </div>
          </div>
          <div className='kontaktDiv'>
            <div className='kontakt'>KONTAKT: {skola.kontakt}</div>
            <div className='kontakt'>
              LOKACIJA: {skola.lokacija}
            </div>
          </div>
        </div>);}
export default Kurs;
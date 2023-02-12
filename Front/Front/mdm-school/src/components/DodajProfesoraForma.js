import React, {Component, useEffect} from 'react';
import './Pocetna.css';
import {Link, Navigate} from 'react-router-dom';
import { useState } from "react";
import { useNavigate,useLocation } from 'react-router-dom';
import axios from 'axios';

function DodajProfesoraForma() {

    const [ime,setIme]=useState("")
    const [prezime,setPrezime]=useState("")
    const [podaci,setPodaci]=useState("")
    const [sSprema,setSprema]=useState("")


    function veliko(string){
        return string.charAt(0).toUpperCase()+string.slice(1);
      }
      
  function dodaj()
  {
    //fetch za dodavanje drzave
    if(ime!="" && prezime!="" && podaci!="" && sSprema!="")
    {
        axios.post(`https://localhost:5001/Profesor/DodajProfesora/${ime}/${prezime}/${podaci}/${sSprema}`)
      .then(res=>{
        alert("Profesor dodat!")
      })
      .catch(err=>{
        alert(err.response.data)
      })
    }
    else{
        alert("Morate popuniti sva polja!!!")
    }
    
  }

  return (
    
    <div className="PretragaForma">
    <div className="Programi">Dodavanje profesora</div>

    <div className="red">
        <label>Ime:</label>
        <input type="text" defaultValue="" name="Naziv"  className='InputIznos'  onChange={(e) => setIme(e.target.value)}></input>
    </div>
    <div className="red">
        <label>Prezime:</label>
        <input type="text" defaultValue="" name="Naziv"  className='InputIznos'  onChange={(e) => setPrezime(e.target.value)}></input>
    </div>
    <div className="red">
        <label>Dodatne informacije:</label>
        <textarea className="inputOpis" type="text" placeholder='Unesite tekst'  onChange={(e) => setPodaci(e.target.value)}></textarea>
    </div>
    <div className="red">
        <label>Strucna sprema:</label>
        <input type="text" defaultValue="" name="Naziv"  className='InputIznos'  onChange={(e) => setSprema(e.target.value)}></input>
    </div>

    <button onClick={()=>dodaj()} className="dugmePretrazi">Dodaj</button>

</div>
    
  )
}

export default DodajProfesoraForma

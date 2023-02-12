import React, {Component, useEffect} from 'react';
import './Pocetna.css';
import {Link, Navigate} from 'react-router-dom';
import { useState } from "react";
import { useNavigate,useLocation } from 'react-router-dom';
import axios from 'axios';

function DodajKategorijuForma() {

    const [naziv,setNaziv]=useState("")

    function veliko(string){
        return string.charAt(0).toUpperCase()+string.slice(1);
      }
      
  function dodaj()
  {
    //fetch za dodavanje kategorije
    if(naziv!="")
    {
        axios.post(`https://localhost:5001/Kategorija/DodajKategoriju/${naziv}`)
      .then(res=>{
        alert("Kategorija dodata!")
      })
      .catch(err=>{
        alert(err.response.data)
      })
    }
    else{
        alert("Unesite naziv kategorije!!!")
    }
    
  }

  return (
    
    <div className="PretragaForma">
    <div className="Programi">Dodavanje kategorije</div>

    <div className="red">
        <label>Naziv:</label>
        <input type="text" defaultValue="" name="Naziv"  className='InputIznos'  onChange={(e) => setNaziv(e.target.value)}></input>
    </div>

    <button onClick={()=>dodaj()} className="dugmePretrazi">Dodaj</button>

</div>
    
  )
}

export default DodajKategorijuForma

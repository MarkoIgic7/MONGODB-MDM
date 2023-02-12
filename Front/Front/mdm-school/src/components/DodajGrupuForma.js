import React, {Component, useEffect} from 'react';
import './Pocetna.css';
import {Link, Navigate} from 'react-router-dom';
import { useState } from "react";
import { useNavigate,useLocation } from 'react-router-dom';
import axios from 'axios';

function DodajGrupuForma() {

    const [naziv,setNaziv]=useState("")
    const [trBroj,setTrBroj]=useState("0")
    const [maxBroj,setMaxBroj]=useState("")
    const [kursevi,setKursevi]=useState([])
    const [kurs, setKurs]=useState("")
    const [profs,setProfs]=useState([])
    const [prof,setProf]=useState("")
    const [termini,setTermini]=useState([])
    const [termin,setTermin]=useState("")

    function veliko(string){
        return string.charAt(0).toUpperCase()+string.slice(1);
      }

      function ukloniTermin(value){
        var array = [...termini]; 
        var index = array.indexOf(value)
        if (index !== -1) {
          array.splice(index, 1);
          setTermini(array);
      }
    }
      function formatTermini()
      {
         var t=""
        termini.forEach(element => {
            t+=element+"*"
        });
        t=t.substring(0,t.length-1)
        return t
      }

      useEffect(()=> {

        axios.get('https://localhost:5001/Kurs/PreuzmiSveKurseve')
        .then(res=>{
          setKursevi(res.data)
        })
        .catch(err=>{
        })

        axios.get('https://localhost:5001/Profesor/PreuzmiSveProfesore')
        .then(res=>{
            setProfs(res.data)
        })
        .catch(err=>{
        })
  
    
      }, [])
      
  function dodaj()
  {
    //fetch za dodavanje grupe
    if(naziv!=""&& maxBroj!=""&&kurs!=""&&prof!=""&&termini.length>0)
    {
        axios.post(`https://localhost:5001/Grupa/DodajGrupu/${naziv}/${trBroj}/${maxBroj}/${kurs}/${prof}/${formatTermini()}`)
      .then(res=>{
        alert("Grupa dodata!")
      })
      .catch(err=>{
        alert(err.response.data)
      })
    }
    else{
        alert("Motrate popuniti sva polja!!!")
    }
    
  }

  return (
    
    <div className="PretragaForma">
    <div className="Programi">Dodavanje grupe</div>

    <div className="red">
        <label>Naziv:</label>
        <input type="text" defaultValue="" name="Naziv"  className='InputIznos'  onChange={(e) => setNaziv(e.target.value)}></input>
    </div>
    <div className="red">
        <label>Maksimalni broj ucenika:</label>
        <input type="text" defaultValue="" name="Naziv"  className='InputIznos'  onChange={(e) => setMaxBroj(e.target.value)}></input>
    </div>
    <div className="red">
                <label>Profesor:</label>
                <select name='Kategorija' className='KategorijaSelect' onChange={e=>setProf(e.target.value)}>
                <option value=' ' selected disabled hidden className='sivo'>Izaberite profesora</option>
                {
                    profs.map((d) => 
                    <option key={d.id} value={d.id}>{veliko(d.ime+" "+d.prezime)}</option>)   
                }
                </select> 
    </div>
    <div className="red">
                <label>Kurs:</label>
                <select name='Kategorija' className='KategorijaSelect' onChange={e=>setKurs(e.target.value)}>
                <option value=' ' selected disabled hidden className='sivo'>Izaberite kurs</option>
                {
                    kursevi.map((d) => 
                    <option key={d.id} value={d.id}>{veliko(d.naziv)}</option>)   
                }
                </select> 
    </div>
    <div className="redTermini">
        <div className='unosTermina'>
        <label>Temin:</label>
        <input type="text" defaultValue="" name="Naziv"  className='InputIznos'  onChange={(e) => setTermin(e.target.value)}></input>
        <button onClick={()=>setTermini((termini)=>[...termini,termin])} className="dugmePretrazi"> Dodaj termin</button>
        </div>
        {termini.length>0 ? (<>
            <ul className='cekiranaPoljaLista'>
                {
                    termini.map((m)=>
                    <div  key={m} className="cekiranoPolje">
                        {<div className='pojedinacniTermin'>{m} </div>}
                        <button className='izkljuciTermin' onClick={()=> ukloniTermin(m)}>x</button>
                    </div>

                    )
                }
            </ul>
            </>) : (<></>)}
    </div>


    <button onClick={()=>dodaj()} className="dugmePretrazi">Dodaj</button>

</div>
    
  )
}

export default DodajGrupuForma

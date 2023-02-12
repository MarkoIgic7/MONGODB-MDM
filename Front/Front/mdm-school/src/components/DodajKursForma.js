import React, {Component, useEffect} from 'react';
import './Pocetna.css';
import {Link, Navigate} from 'react-router-dom';
import { useState } from "react";
import { useNavigate,useLocation } from 'react-router-dom';
import axios from 'axios';

function DodajKursForma() {


    const [naziv,setNaziv]=useState("")
    const [jezik,setJezik]=useState("")
    const [kOpis,setKOpis]=useState("")
    const [kateg,setKateg]=useState("")
    const [prof, setProf]=useState("")
    const [cena, setCena]=useState("")
    const [duziOpis,setDuziOpis]=useState("")
    const[kategs,setKategs]=useState([])
    const [profs,setProfs]=useState([])
    const [termini,setTermini]=useState([])
    const [termin,setTermin]=useState("")
    
    

    useEffect(()=> {

        axios.get('https://localhost:5001/Kategorija/PreuzmiSveKategorije')
        .then(res=>{
          setKategs(res.data)
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

    function veliko(string){
        return string.charAt(0).toUpperCase()+string.slice(1);
      }
      

      
  function dodaj()
  {
    console.log(formatTermini())
     
    //fetch za dodavanje kursa
    if(naziv!="" && jezik!="" && kOpis!="" && kateg!=""&&cena!=""&&duziOpis!="")
    {
        axios.post(`https://localhost:5001/Kurs/DodajKurs/${naziv}/${jezik}/${kOpis}/${kateg}/${cena}/${duziOpis}`)
      .then(res=>{
        alert("Kurs dodat!")
      })
      .catch(err=>{
        alert(err.response.data)
      })
    }
    else{
        alert("Morate popuniti sva polja!!!")
    }
    
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

  return (
    
    <div className="PretragaForma">
    <div className="Programi">Dodavanje kursa</div>

    <div className="red">
        <label>Naziv:</label>
        <input type="text" defaultValue="" name="Naziv"  className='InputIznos'  onChange={(e) => setNaziv(e.target.value)}></input>
    </div>
    <div className="red">
        <label>Jezik:</label>
        <input type="text" defaultValue="" name="Naziv"  className='InputIznos'  onChange={(e) => setJezik(e.target.value)}></input>
    </div>
    <div className="red">
        <label>Kraci opis:</label>
        <textarea className="inputOpis" type="text" placeholder='Unesite tekst'  onChange={(e) => setKOpis(e.target.value)}></textarea>
    </div>
    <div className="red">
                <label>Kategorija:</label>
                <select name='Kategorija' className='KategorijaSelect' onChange={e=>setKateg(e.target.value)}>
                <option value=' ' selected disabled hidden className='sivo'>Izaberite kategoriju</option>
                {
                    kategs.map((d) => 
                    <option key={d.id} value={d.id}>{veliko(d.uzrast)}</option>)   
                }
                </select> 
            </div>
    <div className="red">
        <label>Cena:</label>
        <input type="text" defaultValue="" name="Naziv"  className='InputIznos'  onChange={(e) => setCena(e.target.value)}></input>
    </div>
    <div className="red">
        <label>Duzi opis:</label>
        <textarea className="inputOpis" type="text" placeholder='Unesite tekst'   onChange={(e) => setDuziOpis(e.target.value)}></textarea>
    </div>
    <button onClick={()=>dodaj()} className="dugmePretrazi">Dodaj</button>

</div>
    
  )
}


export default DodajKursForma

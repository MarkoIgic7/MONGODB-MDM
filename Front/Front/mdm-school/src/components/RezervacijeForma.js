import React, {Component, useEffect} from 'react';
import './Pocetna.css';
import {Link, Navigate} from 'react-router-dom';
import { useState } from "react";
import { useNavigate,useLocation } from 'react-router-dom';
import axios from 'axios';
import KarticaRezervacija from './KarticaRezervacija';

function RezervacijeForma() {

  const [rezervacije,setRezervacije] =useState([])

  useEffect(()=> {

    axios.get('https://localhost:5001/Rezervacija/PreuzmiSveRezervacije')
    .then(res=>{
      setRezervacije(res.data)
    })
    .catch(err=>{
    })

  }, [])

  function veliko(string){
    return string.charAt(0).toUpperCase()+string.slice(1);
  }
  function brisanjeRezervacije(id){
        axios.delete(`https://localhost:5001/Rezervacija/ObrisiRezervaciju/${id}`)
        .then(res=>{
          axios.get('https://localhost:5001/Rezervacija/PreuzmiSveRezervacije')
            .then(res=>{
                setRezervacije(res.data)
    })
    .catch(err=>{
    })
          //etRezervacije([])
          alert("Uspesno ste obrisali rezervaciju")
        })
        .catch(err=>{
        })
  }
    
  return (
    
   
    <div><ul>
              {
                  rezervacije.map((el) => 
                  <div className='Rezervacija'>
                      <KarticaRezervacija key={el.id} propvest={el}  onDelete={()=>brisanjeRezervacije(el.id)}></KarticaRezervacija>
                  </div>
                 
      
                  )
              }
          </ul></div>
    
  )
}

export default RezervacijeForma

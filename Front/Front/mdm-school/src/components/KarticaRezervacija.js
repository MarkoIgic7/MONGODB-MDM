import React, {Component, useEffect} from 'react';
import './Navbar.css';
import './KarticaKurs.css';
import './Pocetna.css'
import {Link, Navigate} from 'react-router-dom';
import { useState } from "react";
import { useNavigate,useLocation } from 'react-router-dom';

function KarticaRezervacija({propvest, onDelete}) {

function brisanjeRezervacije(idVesti){
  onDelete(idVesti)
  }
  
  return (
        <div className='KarticaRezervacija'>

          <div className='jezikKurs'>Rezervacija</div>
          <div className="NaslovVest">Rezervisao: {propvest.korisnik}</div>
          <div className="NaslovVest">Grupa: {propvest.grupa}</div>
            {propvest.status? <button className="dugmeObrisiRez" onClick={()=>brisanjeRezervacije(propvest.id)}>Obrisi rezervaciju</button>:<></>}
          
        </div>
  )
}

export default KarticaRezervacija
import React, {Component, useEffect} from 'react';
import './Navbar.css';
import './KarticaKurs.css';
import {Link, Navigate} from 'react-router-dom';
import { useState } from "react";
import { useNavigate,useLocation } from 'react-router-dom';

function KarticaVest({propvest}) {

 
    
 const navigate = useNavigate();
 const goTo = () => {
     navigate(`/${propvest.id}`);
}

  
  return (
        <div className='KarticaVest'>

          <div className='jezikKurs'>{propvest.jezik}</div>
          <div className="NaslovVest">{propvest.naziv}</div>
          <div className="TekstVest">{propvest.kratakOpis}</div>
          <div className="dugmeStrelica" onClick={goTo}>Detaljnije</div>
        </div>
  )
}

export default KarticaVest
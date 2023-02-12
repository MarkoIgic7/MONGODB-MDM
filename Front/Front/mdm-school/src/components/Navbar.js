import React, {Component, useEffect} from 'react';
import './Navbar.css';
import './Button.css'
import {Link, Navigate} from 'react-router-dom';
import { useState } from "react";
import { useNavigate,useLocation } from 'react-router-dom';
import { Login } from './Login';
import { Register } from './Register';
import { useUserContext } from '../context/UserContext';

function Navbar() {

    const navigate = useNavigate();
    const {logOut}=useUserContext();
    const {email, jePosetilac}=useUserContext();

  return (
    <nav className='NavbarStyle'>
          
            <div className='Naslov'>MDMSchool</div>
            {jePosetilac ? ( <><label className="labelaUsername1" onClick={() => {navigate("/Pocetna");}}>Pocetna</label></> ) : <></>}
            { !jePosetilac ? (  
                <>
                {localStorage.getItem("role")=="Admin"?<label className="labelaUsername" onClick={() => {navigate("/Admin");}} >Admin pocetna</label>:
                <label className="labelaUsername" onClick={() => {navigate("/Pocetna");}} >Pocetna </label>}
                <label className='username'>{localStorage.getItem("user")}</label>
                <button className="odjaviSe" onClick={() => {
                    logOut();
                    window.localStorage.removeItem("user");
                    window.localStorage.removeItem("role");
                    window.localStorage.removeItem("id");
                    navigate("/Pocetna");
                    window.location.reload(false);
                    
                  }}
                >
                  Odjavi se
                </button>
                </>
              ) : (
                <div className='dugmiciPrijava'>
                  <Login>Prijava</Login>
                  <Register>Registracija</Register>
                </div>
              )}
        </nav>
  )
}

export default Navbar
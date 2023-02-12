import React, {Component, useEffect} from 'react';
import {Link, Navigate} from 'react-router-dom';
import { useState } from "react";
import { useNavigate,useLocation } from 'react-router-dom';
import DodajKategorijuForma from './DodajKategorijuForma';
import './Admin.css'
import DodajProfesoraForma from './DodajProfesoraForma';
import DodajKursForma from './DodajKursForma';
import RezervacijeForma from './RezervacijeForma';
import axios from 'axios';
import PretraziKursForma from './PretraziKursForma';
import DodajGrupuForma from './DodajGrupuForma';

function Admin (){
    const [forma1,setForma1]=useState(false)
    const [forma2,setForma2]=useState(false)
    const [forma3,setForma3]=useState(false)
    const [forma4,setForma4]=useState(false)
    const [forma5,setForma5]=useState(false)
    const [forma6,setForma6]=useState(false)

    const [skola,setSkola]=useState("")
  
    useEffect(()=> {

      axios.get('https://localhost:5001/Skola/PreuzmiSkolu')
      .then(res=>{
        setSkola(res.data)
      })
      .catch(err=>{
        console.log(err)
      })
    
      }, [])
    function otvoriFormu1()
    {
      setForma1(true)
      setForma2(false)
      setForma3(false)
      setForma4(false)
      setForma5(false)
      setForma6(false)
      
    }

    function otvoriFormu2()
    {
        setForma1(false)
        setForma2(true)
        setForma3(false)
        setForma4(false)
        setForma5(false)
        setForma6(false)
        
    }
    function otvoriFormu3()
    {
        setForma1(false)
      setForma2(false)
      setForma3(true)
      setForma4(false)
      setForma5(false)
      setForma6(false)
     
    }
    function otvoriFormu4()
    {
        setForma1(false)
      setForma2(false)
      setForma3(false)
      setForma4(true)
      setForma5(false)
      setForma6(false)
      
      
    }

    function otvoriFormu5()
    {
        setForma1(false)
      setForma2(false)
      setForma3(false)
      setForma4(false)
      setForma5(true)
      setForma6(false)
      

    }
    function otvoriFormu6()
    {
        setForma1(false)
      setForma2(false)
      setForma3(false)
      setForma4(false)
      setForma5(false)
      setForma6(true)
      
    }

return (
    
        <div className="Pocetna">
            <div className="slika"></div>
            <div className="KarticePretrage">
                <div className='KarticaPretraga1' onClick={()=>{otvoriFormu5()}}>Pretrazi kurseve</div>
                <div className='KarticaPretraga1' onClick={()=>{otvoriFormu1()}}>Dodaj katgoriju</div>
                <div className='KarticaPretraga1' onClick={()=>{otvoriFormu2()}}>Dodaj profesora</div>
                <div className='KarticaPretraga1' onClick={()=>{otvoriFormu3()}}>Dodaj kurs</div>
                <div className='KarticaPretraga1' onClick={()=>{otvoriFormu6()}}>Dodaj grupu</div>
                <div className='KarticaPretraga1' onClick={()=>{otvoriFormu4()}}>Rezervacije</div>
            </div>
            {forma5 ? (<>
                <PretraziKursForma></PretraziKursForma>
            </>)  :  
            (<></>)}
            {forma1 ? (<>
                <DodajKategorijuForma></DodajKategorijuForma>
            </>)  :  
            (<></>)}
             {forma2 ? (<>
                <DodajProfesoraForma></DodajProfesoraForma>
            </>)  :  
            (<></>)}
            {forma3 ? (<>
                <DodajKursForma></DodajKursForma>
            </>)  :  
            (<></>)}
            {forma6 ? (<>
                <DodajGrupuForma></DodajGrupuForma>
            </>)  :  
            (<></>)}
            {forma4 ? (<>
                <RezervacijeForma></RezervacijeForma>
            </>)  :  
            (<></>)}
    
    <div className='kontaktDiv'>
            <div className='kontakt'>KONTAKT: {skola.kontakt}</div>
            <div className='kontakt'>
              LOKACIJA: {skola.lokacija}
            </div>
          </div> 
        </div>
      )
    
}
export default Admin;
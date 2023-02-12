import React, {Component, useEffect} from 'react';
import './Navbar.css';
import './KarticaKurs.css';
import {Link, Navigate} from 'react-router-dom';
import { useState } from "react";
import { useNavigate,useLocation } from 'react-router-dom';
import axios from 'axios';

function KarticaGrupa({prop,kurs}) {
const [nazivGrupe,setNazivGrupe]=useState("")
const [max,setMax]=useState("")
const [trenutno,setTrenutno]=useState("")  
const [forma2, setForma2] = useState(false);
const [termini,setTermini]=useState(prop.termini)
const [termin,setTermin]=useState("")
const [grupa,setGrupa]=useState(prop)
const [formatiranTermin,setFormatiranTermin]=useState("")

  function rezervisi(grupaId){

    console.log(grupaId)
    console.log(localStorage.getItem("id"))
    if(localStorage.getItem("id")!=null)
      {
          axios.post(`https://localhost:5001/Rezervacija/DodajRezervaciju/${grupaId}/${localStorage.getItem("id")}`)    
          .then(res=>{
            alert("Uspesno ste rezervisali mesto u ovoj grupi. Imate 48h da izvrsite uplatu, nakon cega vasa rezervacija moze biti ponistena")
          })
          .catch(err=>{
            if(err.response.status==400)
              {
                alert(err.response.data);
              }
          })
         
      }
    else
      {
          alert("Morate se prijaviti da biste rezervisali mesto!!")
      }
  }

  function ukloniTermin(value){
    
    var array = [...termini];
    var index = array.indexOf(value)
    if (index !== -1) 
    {
      array.splice(index, 1);
      setTermini(array);
      console.log(termini)
    }
    }
    function formatTermini()
      {
         var t=""
         if(termini.length==0)
         {
            grupa.termini.forEach(element => {
            t+=element+"*"
            });

        }
        else{
            termini.forEach(element => {
                t+=element+"*"
            });
        }
        t=t.substring(0,t.length-1)
        console.log(t)
        return t
      }


    function izmeni2(){
   
            axios.put(`https://localhost:5001/Grupa/IzmeniGrupu/${prop.grupaId}/${nazivGrupe==""?grupa.naziv:nazivGrupe}/${trenutno==""?grupa.trenutniBroj:trenutno}/${max==""?grupa.maximalniBroj:max}/${formatTermini()}`)    
        .then(res=>{
          setGrupa(res.data)
          //console.log('izmeni');
          setForma2(!forma2 );
        })
        .catch(err=>{
          console.log(err)
        })
      }


      if(forma2) {
        document.body.classList.add('active-modal')
      } 
      else {
        document.body.classList.remove('active-modal')
      }
      const toggleForma2 = () => {
        setForma2(!forma2);
        setTermini(prop.termini)
      };
  
  return (
    <div className='GrupaDiv'>
    <h3 className='grupa'>{grupa.naziv}</h3>
  
      {localStorage.getItem("role")=="Admin"?
              <button className="btnIzmeniGrupu"  onClick={toggleForma2} >Izmeni informacije o grupi</button>:<></>}
       {forma2 && (
        <div className="modal">
          <div className="overlay"></div>
          <div className="login_modal-content_izmeni">
            
            <i className="fa-solid fa-x close-modal " onClick={toggleForma2}></i>
              <div>Izmeni osnovne informacije o grupi</div>
            <div className="red">
          <label>Naziv:</label>
          <input type="text" defaultValue={grupa.naziv} name="Naziv"  className='InputIznos'  onChange={(e) => setNazivGrupe(e.target.value)}></input>
          </div>
          <div className="red">
          <label>Trenutno upisano:</label>
          <input type="text" defaultValue={grupa.trenutniBroj} name="Naziv"  className='InputIznos'  onChange={(e) => setTrenutno(e.target.value)}></input>
          </div>
          <div className="red">
          <label>Maksimalni kapacitet grupe:</label>
          <textarea className="inputOpis" defaultValue={grupa.maximalniBroj} type="text" placeholder='Unesite tekst'  onChange={(e) => setMax(e.target.value)}></textarea>
          </div>
      <div className="redTermini">
          <div className='unosTermina'>
          <label>Termin:</label>
          <input type="text" defaultValue="" name="Naziv"  className='InputIznos'  onChange={(e) => setTermin(e.target.value)}></input>
          <button onClick={()=>setTermini((termini)=>[...termini,termin])} className="dugmePretrazi"> Dodaj termin</button>
          </div>
          {termini.length>0 ? (<>
              <ul className='cekiranaPoljaLista'>
                  {
                      termini.map((m)=>
                      <div  key={m} className="cekiranoPolje">
                          {<div className='pojedinacniTermin'>{m} </div>}
                          <button className='izkljuciTermin' onClick={()=>ukloniTermin(m)}>x</button>
                      </div>
  
                      )
                  }
              </ul>
              </>) : (<></>)}
      </div>
      <button onClick={()=>izmeni2()} className="dugmePretrazi">Izmeni</button>
  
          </div>
        </div>
      )}      
      <div className='terminiDiv'>
                  <div className='termini'>TERMINI:</div> 
                  <ul className='terminiUl'>
                    {termini.map(t=>
                    <div>{t}</div>
                      )}
                  </ul>
                  <div className='cenaRezervisiDiv'>
                  <div className='cenaKursDiv'>CENA: {kurs.cena} RSD</div>
                  { localStorage.getItem("role")!=null && localStorage.getItem("user")!="admin@gmail.com" ?
                    <div><button className='rezervisiBtn' onClick={()=>rezervisi(grupa.grupaId)}>REZERVISI MESTO</button></div>:
                    <div className='prijavaInfo'>Ukoliko zelite da rezervisete mesto za ovaj kurs morate se prijaviti kao korisnik!!</div>
                  }
              </div>
              </div>
              <div className='profesoriDiv'>
                <h4 className='infoProfNaslov'>Informacije o profesoru na ovom kursu</h4>
                <div className='profInfoDiv'>
                  <div className='prof'>{grupa.profesorIme} {grupa.profesorPrezime} , {grupa.profesorStrucnaSprema}</div>
                  <div className='prof'>{grupa.profesorPodaci}</div>
                </div>
              </div>
              </div>
      
  )
}

export default KarticaGrupa
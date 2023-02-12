import React from 'react';
import { Navigate, useNavigate } from 'react-router-dom';
import { useState, useEffect, useRef } from "react";
import axios from 'axios';
import { useUserContext } from '../context/UserContext';
import './Pocetna.css'
import KarticaKurs from './KarticaKurs'

function PretraziKursForma() {

    const [kategorija,setKategorija]=useState("nema")
    const [kategorije,setKategorije]=useState([])
    const [jezik,setJezik]=useState("nema")
    const [jezici,setJezici]=useState([])
    const [kursevi,setKursevi]=useState([])

    useEffect(()=> {
          //FETCH ZA KATEGORIJE
        axios.get('https://localhost:5001/Kategorija/PreuzmiSveKategorije')
        .then(res=>{
          setKategorije(res.data)
          console.log(kategorije)
        })
        .catch(err=>{
          console.log(err)
        })

        //FETCH ZA JEZIKE!!
        axios.get('https://localhost:5001/Kurs/PreuzmiSveJezike')    
        .then(res=>{
          setJezici(res.data)
        })
        .catch(err=>{
          console.log(err)
        })
  
        
      }, [])

    function veliko(string){
        return string.charAt(0).toUpperCase()+string.slice(1);
      }
      
  function pretrazi()
  {
    if(kategorija!="nema")
    {
      console.log(kategorija);
      if (jezik!="nema")
      {
        axios.get(`https://localhost:5001/Kurs/PreuzmiSveKurseve/${kategorija}/${jezik}`)
        .then(res=>{
          setKursevi(res.data)
          console.log(res.data)
        })
        .catch(err=>{
          if(err.response.status==400)
        {
          setKursevi([])
          alert(err.response.data);
        }
      })
    }
    else
    {
     
        axios.get(`https://localhost:5001/Kurs/PreuzmiSveKurseve/${kategorija}/nema`)
        .then(res=>{
          setKursevi(res.data)
          console.log(res.data)
        })
        .catch(err=>{
          if(err.response.status==400)
          {
            setKursevi([])
            alert(err.response.data);
          }
      })
    }
  }
    else{
      console.log("IMA")
        axios.get(`https://localhost:5001/Kurs/PreuzmiSveKurseve/nema/nema`)
        .then(res=>{
          setKursevi(res.data)
          console.log(kursevi)
        })
        .catch(err=>{
          alert("Morate izabrati barem kategoriju!!")
      })

    }
  }

  return (
    <div className='PretragaDiv'>
    <>
    <div className="PretragaForma">
    <div className="Programi">Pretrazi kurseve</div>

    <div className="red">
        <label>Kategorija:</label>
        <select name='Kategorija' className='KategorijaSelect' onChange={e=>setKategorija(e.target.value)}>
                <option value=' ' selected disabled hidden className='sivo'>Izaberite kategoriju</option>
                {
                    kategorije.map((d) => 
                    <option key={d.id} value={d.id}>{veliko(d.uzrast)}</option>)
                }
                </select> 
    </div>
    <div className="red">
        <label>Jezik:</label>
        <select name='Kategorija' className='KategorijaSelect' onChange={e=>setJezik(e.target.value)}>
                <option value=' ' selected disabled hidden className='sivo'>Izaberite jezik</option>
                {
                    jezici.map((d) => 
                    <option key={d} value={d}>{veliko(d)}</option>)  
                }
                </select> 
    </div>

    <button onClick={()=>pretrazi()} className="dugmePretrazi">Pretrazi</button>

</div>
</>
<ul>
              {
                  kursevi.map((el) => 
                  <div className='VestDiv'>
                      <KarticaKurs key={el.id} propvest={el}  /*onDelete={brisanjeVesti}*/></KarticaKurs>
                  </div>
                 
      
                  )
              }
          </ul>
</div>
 
  )
}

export default PretraziKursForma
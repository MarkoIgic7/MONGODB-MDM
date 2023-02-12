import React from 'react';
import { useState } from 'react';
import './Button.css';
import './LoginForm.css';
import {useNavigate} from 'react-router-dom';
import { useUserContext } from '../context/UserContext';
import axios from 'axios';

const STYLES = [
  'btnn--primary',
  'btnn--outline'
]
const SIZES = [
  'btnn--medium',
  'btnn--large'
]
export const Register = ({
  children,
  type,
  onClick,
  buttonStyle,
  buttonSize
}) => {
  const [forma, setForma] = useState(false);
    const [email,setEmail]=useState("");
    const [password,setPassword]=useState("");
    const [ime, setIme]=useState("");
    const [prezime, setPrezime]=useState("");
    const [tel,setTel]=useState("");
    const navigate=useNavigate();

    const {logIn}=useUserContext();

  const toggleForma = () => {
    setForma(!forma);
  };

  const logovanje = () => {
    email!=="" && password!=="" ? (
      axios.post(`https://localhost:5001/User/Register/${email}/${password}`)   
      .then((response)=>{
        localStorage.setItem("user",response.data.mail);
        localStorage.setItem("role",response.data.uloga)
        localStorage.setItem("id",response.data.id)   //proveri ovo
        logIn(email, response.data.uloga);       //proveri i ovo
        setForma(!forma);
        if(email=="admin@gmail.com")
        {
        navigate('./Admin');
        }
        else
        {
          navigate('/Pocetna' );
        }
        
      })
      .catch(err=>{
        alert(err.response.data)
      })
      ) : alert("Niste popunili sva polja!")
     
  }

  if(forma) {
    document.body.classList.add('active-modal')
  } else {
    document.body.classList.remove('active-modal')
  }
  

  const checkButtonStyle = STYLES.includes(buttonStyle) ? buttonStyle : STYLES[0]
  const checkButtonSize = SIZES.includes(buttonSize) ? buttonSize : SIZES[0]


  return(
   <>
    <button className={`btnn ${checkButtonStyle} ${checkButtonSize}`} onClick={toggleForma}
    type={type}>
      {children}
    </button>
     {forma && (
        <div className="modal"><div className="overlay"></div>
        <div className="login_modal-content">
          
          
            <i className="fa-solid fa-x close-modal " onClick={toggleForma}></i>
         
            <div className="KorisnikIkona"><i className="fa-solid fa-user-plus fa-5x"></i></div>
            <br/>

            <div className='login_red'>
              <label className='login_labelaForma'>E-mail:</label>
              <input className='login_inputTekst' type='text' placeholder='Email' onChange={(e)=>setEmail(e.target.value)}></input>
            </div>
            
            <div className='login_red'>
              <label className='login_labelaForma'>Lozinka:</label>
              <input className='login_inputTekst' type='password' placeholder='password' onChange={(e)=>setPassword(e.target.value)}></input>
            </div>
            <button className="btnRegistracija" onClick={logovanje}>
              Registruj se
            </button>
          
          </div>
        </div>
      )}
  </>
  )
}
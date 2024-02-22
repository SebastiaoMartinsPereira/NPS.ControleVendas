import { Component, OnInit, OnDestroy } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { finalize } from 'rxjs';
import { AuthApiService } from 'src/app/services/auth-api.service'; 

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit, OnDestroy {
  formLogin: FormGroup;
  constructor(
    private authApiService: AuthApiService,
    private fb: FormBuilder
  ) {
    this.formLogin = this.fb.group({
      email: new FormControl('', [Validators.required, Validators.minLength(5)]), 
      password:  new FormControl('',[Validators.required, Validators.minLength(5)]),
    });
  }
  onLogin(password: any, login: any) {

    if(!this.formLogin.valid) {console.log("campos inválidos"); return; }
    console.log(password, login);
    if (password.value && login.value) {
      this.authApiService.getToken(login, password).pipe(
        finalize(() => {
          console.log("funcionou");
        })
      ).subscribe({
        complete: console.info,
        error: (error)=>{
          console.log(error)
        }
      })
    }
  }


  ngOnInit() {
  }

  get email() {
    return this.formLogin.get('email');
  }

  get password() {
    return this.formLogin.get('password');
  }

  ngOnDestroy() { }

  getFieldError(formControlName: string) {

    // Recupera o campo pelo seu nome
    let control = this.formLogin.get(formControlName)

    // Retorna false caso o campo não foi encontrado
    if (!control) return false;

    // Retorna false caso campo ainda não sofreu interação
    if(control.untouched) return false;

    // Retorna false caso campo está válido
     if(control.valid || !control.errors) return false;

    // Retorna mensagem para caso validação de required
    if (control.hasError("required")) {
      return "Este campo é obrigatório.";
    }

    // Retorna mensagem para caso validação de minLength
    if (control.hasError("minlength")) {
      let minlength = control.getError("minlength");
      return "O mínimo de caracteres para este campo é " + minlength.requiredLength + ".";
    }

    // Caso campo é inválido mas erro não foi capturado acima
    return "Campo inválido.";

  }

}

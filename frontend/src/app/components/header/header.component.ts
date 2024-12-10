import { ChangeDetectionStrategy, Component, ElementRef, inject, signal, ViewChild } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatDialog, MatDialogActions, MatDialogClose, MatDialogContent, MatDialogRef, MatDialogTitle } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { Router } from '@angular/router';
import { LoginService } from '../../services/login/login.service';
import { FormsModule } from '@angular/forms';
import { LoginResponseDTO } from '../../models/login-response';
import { RegisterDTO } from '../../models/register';
import { RegisterResponseDTO } from '../../models/register-response';

@Component({
  selector: 'app-header',
  standalone: true,
  imports: [MatButtonModule, MatCardModule],
  templateUrl: './header.component.html',
  styleUrl: './header.component.scss'
})
export class HeaderComponent {
  readonly dialog = inject(MatDialog);

  constructor(private router: Router, private loginService: LoginService) {}

  onSchedules(): void{
    this.router.navigate(['/schedulesBuses']);
  }

  onTitle(): void{
    this.router.navigate(['/mainPage']);
  }

  goToProfile(){
    this.router.navigate(['/account']);
  }

  performAction(){
    if(!this.loginService.checkLoginStatus()) this.openDialog('300ms', '0ms');
    else {
      this.loginService.logout();
      if(this.router.url == "/account") this.onTitle();
      alert("You have been succesfully logged out");
    }
  }

  openDialog(enterAnimationDuration: string, exitAnimationDuration: string): void {
    this.dialog.open(LoginDialog, {
      width: '250px',
      enterAnimationDuration,
      exitAnimationDuration,
    });
  }

  checkLogin() : boolean{
    return this.loginService.checkLoginStatus();
  }
}

@Component({
  selector: 'login-dialog',
  templateUrl: './login-dialog.html',
  styleUrl: './login-dialog.scss',
  standalone: true,
  imports: [MatButtonModule, 
            MatDialogActions, 
            MatDialogClose, 
            MatDialogTitle, 
            MatDialogContent,
            MatFormFieldModule, 
            MatInputModule,
            MatIconModule,
            FormsModule],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class LoginDialog {
  readonly dialog = inject(MatDialog);
  readonly dialogRef = inject(MatDialogRef<LoginDialog>);
  hide = signal(true);
  loginData = { Login: '', Password: ''};
  constructor(private loginService: LoginService, private router: Router){}

  clickEvent(event: MouseEvent) {
    this.hide.set(!this.hide());
    event.stopPropagation();
  }
  login(){
    
    this.loginService.login(this.loginData).subscribe(
      (response: LoginResponseDTO) => {
        localStorage.setItem('userData', JSON.stringify(response.user)); 
        localStorage.setItem('jwt', response!.token); // Store the JWT token
        localStorage.setItem('jwt_expiration', response!.expirationTime.toString());
        console.log('Login successful');
        this.dialogRef.close();
      },
      (error) => {
        console.log('Login failed');
        alert('bad username or passsword');
      }
    );
  }
  createAccount(enterAnimationDuration: string, exitAnimationDuration: string){
    this.dialog.open(RegisterDialog, {
      width: '250px',
      enterAnimationDuration,
      exitAnimationDuration,
    });
  }
}

@Component({
  selector: 'register-dialog',
  templateUrl: './register-dialog.html',
  styleUrl: './register-dialog.scss',
  standalone: true,
  imports: [MatButtonModule, 
            MatDialogActions, 
            MatDialogClose, 
            MatDialogTitle, 
            MatDialogContent,
            MatFormFieldModule, 
            MatInputModule,
            MatIconModule,
            FormsModule],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class RegisterDialog {
  readonly dialogRef = inject(MatDialogRef<RegisterDialog>);
  hide = signal(true);
  registerInfo : RegisterDTO = {
    userName: "",
    userSurname: "",
    password: ""
};
  constructor(private loginService: LoginService, private router: Router){}

  clickEvent(event: MouseEvent) {
    this.hide.set(!this.hide());
    event.stopPropagation();
  }
  register(){
    
    this.loginService.register(this.registerInfo!).subscribe((response : RegisterResponseDTO)=> {
      if(response.message != "Registration successful"){
        alert("something went wrong, try again");
      }
    })
    this.dialogRef.close();
  } 
}

import { Component, OnInit, ViewChild } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { AuthService } from '../auth.service';
import { Router } from '@angular/router';
import { Login } from '../model/login.model';

@Component({
  selector: 'xp-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css'],
})
export class LoginComponent implements OnInit {

  constructor(
    private authService: AuthService,
    private router: Router
  ) {}

  isDisabled: boolean = false;

  ngOnInit(): void {
  }

  loginForm = new FormGroup({
    username: new FormControl('', [Validators.required]),
    password: new FormControl('', [Validators.required]),
  });

  login(): void {
    const login: Login = {
      email: this.loginForm.value.username || '',
      password: this.loginForm.value.password || '',
    };

    if (this.loginForm.valid) {
      this.isDisabled = true;
      this.authService.login(login).subscribe({
        next: () => {
          this.router.navigate(['/chat2']);
          this.isDisabled = false;
        },
        error: (err) => {
          this.isDisabled = false;
        },
      });
    } else {
    }
  }
}

import { Component, signal } from '@angular/core';
import { FormsModule } from '@angular/forms'
import { inject } from '@angular/core';
import { AccountService } from '../../core/services/account-service';
import { Router, RouterLink, RouterLinkActive } from "@angular/router";
import { ToastService } from '../../core/services/toast-service';

@Component({
  selector: 'app-nav',
  imports: [FormsModule, RouterLink, RouterLinkActive],
  templateUrl: './nav.html',
  styleUrl: './nav.css',
})
export class Nav {
  protected accountService = inject(AccountService);
  protected cred: any = {};
  private toast = inject(ToastService);
  protected router = inject(Router);

  login(){
    this.accountService.login(this.cred).subscribe({
      next: () => {
        this.router.navigateByUrl('/members');
        this.toast.success('logged in successfully');
        this.cred={};
      },
      error: error => {
        this.toast.error(error.error);
      }
    })
  }

  logout(){
    this.router.navigateByUrl('/');
    this.accountService.logout();
  }
}

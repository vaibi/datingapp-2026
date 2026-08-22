import { Component, OnInit, signal } from '@angular/core';
import { FormsModule } from '@angular/forms'
import { inject } from '@angular/core';
import { AccountService } from '../../core/services/account-service';
import { Router, RouterLink, RouterLinkActive } from "@angular/router";
import { ToastService } from '../../core/services/toast-service';
import { themes } from '../themes';
import { BusyService } from '../../core/services/busy-service';

@Component({
  selector: 'app-nav',
  imports: [FormsModule, RouterLink, RouterLinkActive],
  templateUrl: './nav.html',
  styleUrl: './nav.css',
})
export class Nav implements OnInit {
  protected accountService = inject(AccountService);
  protected busyService = inject(BusyService);
  protected cred: any = {};
  private toast = inject(ToastService);
  protected router = inject(Router);
  protected selectedThemes = signal<string>(localStorage.getItem('theme') || 'light');
  protected themes = themes;

  ngOnInit(): void {
    document.documentElement.setAttribute('data-theme', this.selectedThemes());
  }

  handleSelectTheme(theme: string) {
    this.selectedThemes.set(theme);
    localStorage.setItem('theme', theme);
    document.documentElement.setAttribute('data-theme', theme);
    const elem = document.activeElement as HTMLDivElement;
    if(elem) elem.blur();
  }

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

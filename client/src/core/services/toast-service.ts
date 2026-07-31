import { Injectable } from '@angular/core';

@Injectable({
    providedIn: 'root'
})

export class ToastService {

    constructor(){
        this.createToastContainer();
    }

    private createToastContainer(){
        if(!document.getElementById('toast-container')){
            const container = document.createElement('div');
            container.id = 'toast-container';
            container.className = 'toast toast-bottom toast-end'
            document.body.appendChild(container)
        }
    }

    private createToastElement(messsage: string, alertClass: string, duration = 5000){
        const toastContainer = document.getElementById('toast-container');
        if(!toastContainer) return;

        const toast = document.createElement('div');
        toast.classList.add('alert', alertClass, 'shadow-lg');
        toast.innerHTML = `
        <span>${messsage}</span>
        <button class="ml-4 btn btn-sm btn-ghost">x</button>
        `

        toast.querySelector('button')?.addEventListener('click', () => {
            toastContainer.removeChild(toast);
        })

        toastContainer.append(toast);

        setTimeout(() => {
           if(toastContainer.contains(toast)){
            toastContainer.removeChild(toast);
           } 
        }, duration);
    }

    success(messsage: string, duration?: number){
        this.createToastElement(messsage, 'alert-success', duration);
    }

    error(messsage: string, duration?: number){
        this.createToastElement(messsage, 'alert-error', duration);
    }

    warning(messsage: string, duration?: number){
        this.createToastElement(messsage, 'alert-warning', duration);
    }

    info(messsage: string, duration?: number){
        this.createToastElement(messsage, 'alert-info', duration);
    }
}

import { Component, OnInit } from '@angular/core';
import { ChatService } from 'src/app/services/chat.service';

@Component({
  selector: 'app-chat',
  templateUrl: './chat.component.html',
  styleUrls: ['./chat.component.css'],
})
export class ChatComponent implements OnInit {
  userId: string = ''; // Current user's ID
  receiverId: string = ''; // ID of the user to send messages to
  message: string = ''; // Message to send
  messages: { senderId: string; text: string }[] = []; // List of received messages


  constructor(private chatService: ChatService) {}

  async ngOnInit(): Promise<void> {
    await this.chatService.connect();

    this.userId = prompt('Enter your User ID:') || '';
    await this.chatService.registerUser(this.userId);

    this.chatService.addMessageListener((senderId, message) => {
      this.messages.push({ senderId, text: message });
    });

    this.chatService.addUserNotAvailableListener((receiverId) => {
      alert(`User ${receiverId} is not available.`);
    });
  }

  async sendMessage(): Promise<void> {
    if (this.receiverId && this.message) {
      await this.chatService.sendMessage(this.receiverId, this.userId, this.message);
      this.messages.push({ senderId: 'Me', text: this.message });
      this.message = ''; // Clear the input
    }
  }
}
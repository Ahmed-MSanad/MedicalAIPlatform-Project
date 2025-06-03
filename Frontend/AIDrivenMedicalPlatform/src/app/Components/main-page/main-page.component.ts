import { Router } from '@angular/router';
import { Component, ElementRef, OnInit, QueryList, ViewChildren } from '@angular/core';
import { CommonModule } from '@angular/common';

interface Feature {
  title: string;
  description: string;
  icon: string;
  color: string;
  bgColor: string;
  details: string[];
}

interface Testimonial {
  name: string;
  role: string;
  content: string;
  rating: number;
  avatar: string;
  location: string;
}

interface Stat {
  number: string;
  label: string;
  icon: string;
  color: string;
}

interface Partner {
  name: string;
  logo: string;
  type: 'certification' | 'hospital' | 'partner';
}

@Component({
  selector: 'app-main-page',
  imports: [CommonModule],
  templateUrl: './main-page.component.html',
  styleUrl: './main-page.component.scss'
})
export class MainPageComponent implements OnInit {
  
  constructor(private _router: Router) { }

  ngOnInit(): void {
    // Initialize intersection observer for scroll animations
    this.setupScrollAnimations();
    this.animateStats();
  }


/* 🟢Section_____________________________________________________________________________________________1🟢 */
  onGetStarted(): void {
    this._router.navigate(["/register"]);
  }
  
  onWatchDemo(): void {
    console.log('Show demo');
  }



/* 🟢Section_____________________________________________________________________________________________3🟢 */

  @ViewChildren('featureCard') featureCards!: QueryList<ElementRef>;

  features: Feature[] = [
    {
      title: 'AI-Powered Analysis',
      description: 'Advanced machine learning algorithms provide accurate diagnoses with confidence scores, detailed heatmaps, and comprehensive explanations.',
      icon: `<svg fill="none" stroke="currentColor" viewBox="0 0 24 24">
        <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M9.663 17h4.673M12 3v1m6.364 1.636l-.707.707M21 12h-1M4 12H3m3.343-5.657l-.707-.707m2.828 9.9a5 5 0 117.072 0l-.548.547A3.374 3.374 0 0014 18.469V19a2 2 0 11-4 0v-.531c0-.895-.356-1.754-.988-2.386l-.548-.547z"></path>
      </svg>`,
      color: 'text-blue-600',
      bgColor: 'bg-blue-100',
      details: ['95% accuracy rate', '3D visualization', 'Real-time processing', 'Multiple image formats']
    },
    {
      title: 'Secure Image Uploads',
      description: 'Your medical images are protected with end-to-end encryption, HIPAA compliance, and military-grade security protocols.',
      icon: `<svg fill="none" stroke="currentColor" viewBox="0 0 24 24">
        <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 15v2m-6 4h12a2 2 0 002-2v-6a2 2 0 00-2-2H6a2 2 0 00-2 2v6a2 2 0 002 2zm10-10V7a4 4 0 00-8 0v4h8z"></path>
      </svg>`,
      color: 'text-green-600',
      bgColor: 'bg-green-100',
      details: ['256-bit encryption', 'HIPAA compliant', 'Secure cloud storage', 'Data anonymization']
    },
    {
      title: 'Connect with Doctors',
      description: 'Seamlessly connect with certified radiologists and specialists. Book appointments, get second opinions, and receive expert consultations.',
      icon: `<svg fill="none" stroke="currentColor" viewBox="0 0 24 24">
        <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M17 20h5v-2a3 3 0 00-5.356-1.857M17 20H7m10 0v-2c0-.656-.126-1.283-.356-1.857M7 20H2v-2a3 3 0 015.356-1.857M7 20v-2c0-.656.126-1.283.356-1.857m0 0a5.002 5.002 0 019.288 0M15 7a3 3 0 11-6 0 3 3 0 016 0zm6 3a2 2 0 11-4 0 2 2 0 014 0zM7 10a2 2 0 11-4 0 2 2 0 014 0z"></path>
      </svg>`,
      color: 'text-purple-600',
      bgColor: 'bg-purple-100',
      details: ['24/7 availability', 'Instant messaging', 'Video consultations', 'Appointment scheduling']
    },
    {
      title: 'Feedback System',
      description: 'Help us improve through our comprehensive feedback system. Rate diagnoses, suggest improvements, and contribute to better AI accuracy.',
      icon: `<svg fill="none" stroke="currentColor" viewBox="0 0 24 24">
        <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M7 8h10M7 12h4m1 8l-4-4H5a2 2 0 01-2-2V6a2 2 0 012-2h14a2 2 0 012 2v8a2 2 0 01-2 2h-3l-4 4z"></path>
      </svg>`,
      color: 'text-orange-600',
      bgColor: 'bg-orange-100',
      details: ['User ratings', 'Improvement suggestions', 'Community feedback', 'AI learning integration']
    }
  ];

  onCardHover(index: number, isHovering: boolean) {
    // Add subtle scale effect on hover
    const card = this.featureCards?.toArray()[index];
    if (card) {
      if (isHovering) {
        card.nativeElement.style.transform = 'translateY(-8px)';
      } else {
        card.nativeElement.style.transform = 'translateY(0)';
      }
    }
  }

  private setupScrollAnimations() {
    // Setup intersection observer for scroll-triggered animations
    if (typeof IntersectionObserver !== 'undefined') {
      const observer = new IntersectionObserver(
        (entries) => {
          entries.forEach((entry) => {
            if (entry.isIntersecting) {
              entry.target.classList.add('animate-fade-in-up');
            }
          });
        },
        { threshold: 0.1 }
      );

      // Observe elements when they become available
      setTimeout(() => {
        const elements = document.querySelectorAll('.animate-on-scroll');
        elements.forEach((el) => observer.observe(el));
      }, 100);
    }
  }







/* 🟢Section_____________________________________________________________________________________________4🟢 */

  currentStats: string[] = ['0', '0', '0', '0'];
  private animationIntervals: any[] = [];

  stats: Stat[] = [
    {
      number: '15,000+',
      label: 'Images Analyzed',
      icon: `<svg fill="none" stroke="currentColor" viewBox="0 0 24 24">
        <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M4 16l4.586-4.586a2 2 0 012.828 0L16 16m-2-2l1.586-1.586a2 2 0 012.828 0L20 14m-6-6h.01M6 20h12a2 2 0 002-2V6a2 2 0 00-2-2H6a2 2 0 00-2 2v12a2 2 0 002 2z"></path>
      </svg>`,
      color: 'text-blue-600'
    },
    {
      number: '98%',
      label: 'Accuracy Rate',
      icon: `<svg fill="none" stroke="currentColor" viewBox="0 0 24 24">
        <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M9 19v-6a2 2 0 00-2-2H5a2 2 0 00-2 2v6a2 2 0 002 2h2a2 2 0 002-2zm0 0V9a2 2 0 012-2h2a2 2 0 012 2v10m-6 0a2 2 0 002 2h2a2 2 0 002-2m0 0V5a2 2 0 012-2h2a2 2 0 012 2v14a2 2 0 01-2 2h-2a2 2 0 01-2-2z"></path>
      </svg>`,
      color: 'text-green-600'
    },
    {
      number: '2,500+',
      label: 'Healthcare Professionals',
      icon: `<svg fill="none" stroke="currentColor" viewBox="0 0 24 24">
        <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M17 20h5v-2a3 3 0 00-5.356-1.857M17 20H7m10 0v-2c0-.656-.126-1.283-.356-1.857M7 20H2v-2a3 3 0 015.356-1.857M7 20v-2c0-.656.126-1.283.356-1.857m0 0a5.002 5.002 0 019.288 0M15 7a3 3 0 11-6 0 3 3 0 016 0zm6 3a2 2 0 11-4 0 2 2 0 014 0zM7 10a2 2 0 11-4 0 2 2 0 014 0z"></path>
      </svg>`,
      color: 'text-purple-600'
    },
    {
      number: '24/7',
      label: 'Support Available',
      icon: `<svg fill="none" stroke="currentColor" viewBox="0 0 24 24">
        <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 8v4l3 3m6-3a9 9 0 11-18 0 9 9 0 0118 0z"></path>
      </svg>`,
      color: 'text-orange-600'
    }
  ];

  testimonials: Testimonial[] = [
    {
      name: 'Dr. Sarah Johnson',
      role: 'Radiologist',
      content: 'This platform helped me detect an early-stage tumor that I almost missed. The AI highlighting feature is incredible - highly recommend!',
      rating: 5,
      avatar: 'bg-blue-500',
      location: 'Mayo Clinic, Rochester'
    },
    {
      name: 'John D.',
      role: 'Patient',
      content: 'Got my X-ray results explained in simple terms with visual highlights. Finally understood my condition and felt confident about my treatment.',
      rating: 5,
      avatar: 'bg-green-500',
      location: 'New York, NY'
    },
    {
      name: 'Dr. Michael Chen',
      role: 'Emergency Physician',
      content: 'The speed and accuracy of diagnoses have transformed our ER workflow. We can now provide faster, more confident care to our patients.',
      rating: 5,
      avatar: 'bg-purple-500',
      location: 'Johns Hopkins Hospital'
    },
    {
      name: 'Emily Rodriguez',
      role: 'Nurse Practitioner',
      content: 'The consultation booking feature is seamless. Patients love being able to connect with specialists directly through the platform.',
      rating: 4,
      avatar: 'bg-pink-500',
      location: 'Cleveland Clinic'
    },
    {
      name: 'Dr. David Kim',
      role: 'Orthopedic Surgeon',
      content: 'The AI analysis provides excellent second opinions. It has caught several cases I wanted to review more thoroughly.',
      rating: 5,
      avatar: 'bg-indigo-500',
      location: 'Stanford Medical Center'
    },
    {
      name: 'Maria Santos',
      role: 'Patient',
      content: 'User-friendly interface and quick results. The peace of mind from getting a professional second opinion is invaluable.',
      rating: 5,
      avatar: 'bg-teal-500',
      location: 'Los Angeles, CA'
    }
  ];

  partners: Partner[] = [
    {
      name: 'HIPAA Certified',
      type: 'certification',
      logo: `<div class="bg-green-100 rounded-lg p-2">
        <svg class="w-8 h-8 text-green-600" fill="currentColor" viewBox="0 0 20 20">
          <path fill-rule="evenodd" d="M2.166 4.999A11.954 11.954 0 0010 1.944 11.954 11.954 0 0017.834 5c.11.65.166 1.32.166 2.001 0 5.225-3.34 9.67-8 11.317C5.34 16.67 2 12.225 2 7c0-.682.057-1.35.166-2.001zm11.541 3.708a1 1 0 00-1.414-1.414L9 10.586 7.707 9.293a1 1 0 00-1.414 1.414l2 2a1 1 0 001.414 0l4-4z" clip-rule="evenodd"></path>
        </svg>
      </div>`
    },
    {
      name: 'AWS Healthcare',
      type: 'partner',
      logo: `<div class="bg-orange-100 rounded-lg p-2">
        <svg class="w-8 h-8 text-orange-600" fill="currentColor" viewBox="0 0 20 20">
          <path d="M3 4a1 1 0 011-1h12a1 1 0 011 1v2a1 1 0 01-1 1H4a1 1 0 01-1-1V4zM3 10a1 1 0 011-1h6a1 1 0 011 1v6a1 1 0 01-1 1H4a1 1 0 01-1-1v-6zM14 9a1 1 0 00-1 1v6a1 1 0 001 1h2a1 1 0 001-1v-6a1 1 0 00-1-1h-2z"></path>
        </svg>
      </div>`
    },
    {
      name: 'Microsoft Azure',
      type: 'partner',
      logo: `<div class="bg-blue-100 rounded-lg p-2">
        <svg class="w-8 h-8 text-blue-600" fill="currentColor" viewBox="0 0 20 20">
          <path d="M2 6a2 2 0 012-2h6a2 2 0 012 2v8a2 2 0 01-2 2H4a2 2 0 01-2-2V6zM14.553 7.106A1 1 0 0014 8v4a1 1 0 00.553.894l2 1A1 1 0 0018 13V7a1 1 0 00-1.447-.894l-2 1z"></path>
        </svg>
      </div>`
    },
    {
      name: 'FDA Cleared',
      type: 'certification',
      logo: `<div class="bg-red-100 rounded-lg p-2">
        <svg class="w-8 h-8 text-red-600" fill="currentColor" viewBox="0 0 20 20">
          <path fill-rule="evenodd" d="M6.267 3.455a3.066 3.066 0 001.745-.723 3.066 3.066 0 013.976 0 3.066 3.066 0 001.745.723 3.066 3.066 0 012.812 2.812c.051.643.304 1.254.723 1.745a3.066 3.066 0 010 3.976 3.066 3.066 0 00-.723 1.745 3.066 3.066 0 01-2.812 2.812 3.066 3.066 0 00-1.745.723 3.066 3.066 0 01-3.976 0 3.066 3.066 0 00-1.745-.723 3.066 3.066 0 01-2.812-2.812 3.066 3.066 0 00-.723-1.745 3.066 3.066 0 010-3.976 3.066 3.066 0 00.723-1.745 3.066 3.066 0 012.812-2.812zm7.44 5.252a1 1 0 00-1.414-1.414L9 10.586 7.707 9.293a1 1 0 00-1.414 1.414l2 2a1 1 0 001.414 0l4-4z" clip-rule="evenodd"></path>
        </svg>
      </div>`
    },
    {
      name: 'Google Cloud',
      type: 'partner',
      logo: `<div class="bg-yellow-100 rounded-lg p-2">
        <svg class="w-8 h-8 text-yellow-600" fill="currentColor" viewBox="0 0 20 20">
          <path fill-rule="evenodd" d="M3 3a1 1 0 000 2v8a2 2 0 002 2h2.586l-1.293 1.293a1 1 0 101.414 1.414L10 15.414l2.293 2.293a1 1 0 001.414-1.414L12.414 15H15a2 2 0 002-2V5a1 1 0 100-2H3zm11.707 4.707a1 1 0 00-1.414-1.414L10 9.586 8.707 8.293a1 1 0 00-1.414 1.414l2 2a1 1 0 001.414 0l4-4z" clip-rule="evenodd"></path>
        </svg>
      </div>`
    },
    {
      name: 'ISO 27001',
      type: 'certification',
      logo: `<div class="bg-purple-100 rounded-lg p-2">
        <svg class="w-8 h-8 text-purple-600" fill="currentColor" viewBox="0 0 20 20">
          <path d="M9 12l2 2 4-4m6 2a9 9 0 11-18 0 9 9 0 0118 0z"></path>
        </svg>
      </div>`
    }
  ];

  ngOnDestroy() {
    this.animationIntervals.forEach(interval => clearInterval(interval));
  }

  private animateStats() {
    // Animate stats counting up
    setTimeout(() => {
      this.stats.forEach((stat, index) => {
        this.animateNumber(stat.number, index);
      });
    }, 1000);
  }

  private animateNumber(target: string, index: number) {
    const isPercentage = target.includes('%');
    const isTime = target.includes('/');
    const numericPart = target.replace(/[^\d]/g, '');
    const targetNum = parseInt(numericPart);
    
    if (isTime) {
      this.currentStats[index] = target;
      return;
    }
    
    let current = 0;
    const increment = Math.ceil(targetNum / 50);
    
    const interval = setInterval(() => {
      current += increment;
      if (current >= targetNum) {
        current = targetNum;
        clearInterval(interval);
      }
      
      if (isPercentage) {
        this.currentStats[index] = current + '%';
      } else {
        this.currentStats[index] = current.toLocaleString() + '+';
      }
    }, 30);
    
    this.animationIntervals.push(interval);
  }








/* 🟢Section_____________________________________________________________________________________________5🟢 */

isAnimating: string | null = null;

  signUpAsPatient(): void {
    this.triggerAnimation('patient');
    // Add your patient sign-up logic here
    console.log('Redirecting to patient sign-up...');
    // Example: this.router.navigate(['/signup/patient']);
  }

  signUpAsDoctor(): void {
    this.triggerAnimation('doctor');
    // Add your doctor sign-up logic here
    console.log('Redirecting to doctor sign-up...');
    // Example: this.router.navigate(['/signup/doctor']);
  }

  signUpAsHospitalAdmin(): void {
    this.triggerAnimation('admin');
    // Add your hospital admin sign-up logic here
    console.log('Redirecting to hospital admin sign-up...');
    // Example: this.router.navigate(['/signup/hospital-admin']);
  }

  private triggerAnimation(type: string): void {
    this.isAnimating = type;
    setTimeout(() => {
      this.isAnimating = null;
    }, 600);
  }









/* 🟢Section_____________________________________________________________________________________________6🟢 */

  currentYear = new Date().getFullYear();

  // Navigation Methods
  navigateToAbout(): void {
    console.log('Navigating to About Us');
    // Example: this.router.navigate(['/about']);
  }

  navigateToContact(): void {
    console.log('Navigating to Contact Us');
    // Example: this.router.navigate(['/contact']);
  }

  navigateToFeatures(): void {
    console.log('Navigating to Features');
    // Example: this.router.navigate(['/features']);
  }

  navigateToSupport(): void {
    console.log('Navigating to Support');
    // Example: this.router.navigate(['/support']);
  }

  navigateToPrivacy(): void {
    console.log('Navigating to Privacy Policy');
    // Example: this.router.navigate(['/privacy']);
  }

  navigateToTerms(): void {
    console.log('Navigating to Terms of Service');
    // Example: this.router.navigate(['/terms']);
  }

  navigateToCookies(): void {
    console.log('Navigating to Cookie Policy');
    // Example: this.router.navigate(['/cookies']);
  }

  navigateToCompliance(): void {
    console.log('Navigating to HIPAA Compliance');
    // Example: this.router.navigate(['/compliance']);
  }

  // Social Media Methods
  openSocialMedia(platform: string): void {
    const urls = {
      twitter: 'https://twitter.com/xaimedical',
      linkedin: 'https://linkedin.com/company/xaimedical',
      facebook: 'https://facebook.com/xaimedical',
      youtube: 'https://youtube.com/xaimedical'
    };
    
    console.log(`Opening ${platform} social media`);
    // Example: window.open(urls[platform], '_blank');
  }

}